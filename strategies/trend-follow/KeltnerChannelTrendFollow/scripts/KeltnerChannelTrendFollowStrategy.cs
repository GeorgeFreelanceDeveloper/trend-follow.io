using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class KeltnerChannelTrendFollow_cBot : Robot
    {
        // ********************************
        // User defined inputs
        // ********************************

        // Basic settings
        [Parameter("EMA Length", Group = "Basic settings", DefaultValue = 20)]
        public int EmaLength { get; set; }

        [Parameter("ATR Breakout Multiplier", Group = "Basic settings", DefaultValue = 10)]
        public int AtrBreakoutLength { get; set; }

        [Parameter("Upper Multiplier", Group = "Basic settings", DefaultValue = 1)]
        public double MultiplierUpper { get; set; }

        [Parameter("Lower Multiplier", Group = "Basic settings", DefaultValue = 0.5)]
        public double MultiplierLower { get; set; }

        [Parameter("Risk Percentage", Group = "Basic settings", DefaultValue = 1)]
        public double RiskPercentage { get; set; }

        [Parameter("ATR Multiplier", Group = "Basic settings", DefaultValue = 2)]
        public int AtrMultiplier { get; set; }

        [Parameter("ATR Length", Group = "Basic settings", DefaultValue = 20)]
        public int AtrLength { get; set; }

        // Filter settings
        [Parameter("Enable Filter", Group = "Filter settings", DefaultValue = false)]
        public bool EnableFilter { get; set; }

        [Parameter("Price above SMA(X)", Group ="Filter settings", DefaultValue =200)]
        public int SmaLength { get; set; }

        [Parameter("RSI > X", Group ="Filter settings", DefaultValue = 0)]
        public int RsiValue { get; set; }

        protected override void OnStart()
        {

        }

        protected override void OnBarClosed()
        {
            // **********************************
            // Perform calculations and analysis
            // **********************************

            // Initialize indicators
            ExponentialMovingAverage _ema = Indicators.ExponentialMovingAverage(Bars.ClosePrices, EmaLength);
            AverageTrueRange _atr = Indicators.AverageTrueRange(AtrBreakoutLength, MovingAverageType.Simple);

            double upperChannel = _ema.Result.LastValue + _atr.Result.LastValue * MultiplierUpper;
            double lowerChannel = _ema.Result.LastValue - _atr.Result.LastValue * MultiplierLower;

            string label = $"KeltnerChannelTrendFollow_cBot-{Symbol.Name}";

            double qty = ((RiskPercentage / 100) * Account.Balance) / (AtrMultiplier * Indicators.AverageTrueRange(AtrLength, MovingAverageType.Simple).Result.LastValue);
            double qtyInLots = ((int)(qty / Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;

            // Filter
            double lastClosePrice = Bars.ClosePrices.LastValue;
            bool priceAboveSMA = lastClosePrice > Indicators.SimpleMovingAverage(Bars.ClosePrices, SmaLength).Result.LastValue;
            bool rsiAboveValue = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14).Result.LastValue > RsiValue;
            bool filter = EnableFilter ? priceAboveSMA && rsiAboveValue : true;

            // Check position
            Position position = Positions.Find(label);
            bool isOpenPosition = position != null;

            // Conditions
            bool buyCondition = lastClosePrice > upperChannel && !isOpenPosition && filter;
            bool sellCondition = lastClosePrice < lowerChannel && isOpenPosition;


            // ********************************
            // Manage trade
            // ********************************
            if (buyCondition)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, qtyInLots, label);
            }

            if (sellCondition)
            {
                ClosePosition(position);
            }

            Print("Sucessful call OnBarClosed() method.");
        }
    }
}
