using System;

using cAlgo.API;
using cAlgo.API.Indicators;


namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TurtleTrendFollow_cBot : Robot
    {
    
        // ********************************
        // User defined inputs
        // ********************************
        
        // Basic settings
        [Parameter("Entry Length", Group ="Basic settings", DefaultValue =20)]
        public int EntryLength {get; set;}
        
        [Parameter("Exit Length", Group ="Basic settings", DefaultValue =10)]
        public int ExitLength {get; set;}
        
        [Parameter("Risk Percentage", Group ="Basic settings", DefaultValue =1)]
        public double RiskPercentage {get; set;}
        
        [Parameter("Atr Multiplier", Group ="Basic settings", DefaultValue =2)]
        public int AtrMultiplier {get; set;}
        
        [Parameter("Atr Length", Group ="Basic settings", DefaultValue =20)]
        public int AtrLength {get;set;}
        
        // Filter settings
        [Parameter("Enable Filter", Group ="Filter settings", DefaultValue =false)]
        public bool EnableFilter {get; set;} 
        
        [Parameter("Price above SMA(X)", Group ="Filter settings", DefaultValue =200)]
        public int SmaLength {get;set;}
        
        [Parameter("RSI > X", Group ="Filter settings", DefaultValue = 70)]
        public int RsiValue {get;set;}
        
        protected override void OnStart()
        {

        }

        protected override void OnBarClosed()
        {
            // **********************************
            // Perform calculations and analysis
            // **********************************
            
            // Basic
            double upperChannel = Indicators.DonchianChannel(EntryLength).Top.LastValue;
            double lowerChannel = Indicators.DonchianChannel(ExitLength).Bottom.LastValue;
            double closePrice = Bars.ClosePrices.LastValue;
            string label = $"TurtleTrendFollow_cBot-{Symbol.Name}";
            
            // Trade amount
            double qty = ((RiskPercentage/100) * Account.Balance) / (AtrMultiplier * Indicators.AverageTrueRange(20,MovingAverageType.Simple).Result.LastValue);
            double qtyInLots = ((int)(qty /Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
            
            // Filter
            bool priceAboveSMA = closePrice > Indicators.SimpleMovingAverage(Bars.ClosePrices, SmaLength).Result.LastValue;
            bool rsiAboveValue = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14).Result.LastValue > RsiValue;
            bool filter = EnableFilter ? priceAboveSMA && rsiAboveValue : true;

            Position position = Positions.Find(label);
            bool isOpenPosition = position != null;
            
            bool buyCondition = closePrice > upperChannel && !isOpenPosition && filter;
            bool sellCondition = closePrice < lowerChannel && isOpenPosition;
            
            // ********************************
            // Manage trade
            // ********************************
            
            // Entry
            if(buyCondition)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, qtyInLots, label);
            }
            
            // Exit
            if(sellCondition)
            {
                position.Close();
            }
         }
    }
}
