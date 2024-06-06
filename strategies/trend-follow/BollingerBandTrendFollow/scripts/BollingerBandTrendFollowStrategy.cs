using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BollingerBandTrendFollow_cBot : Robot
    {
        // ********************************
        // User defined inputs
        // ********************************
        
        // Basic settings
        [Parameter("Length", Group ="Basic settings", DefaultValue = 20)]
        public int Length { get; set; }
        
        [Parameter("Upper", Group ="Basic settings", DefaultValue = 1)]
        public int Upper { get; set; }
        
        [Parameter("Lower", Group ="Basic settings", DefaultValue = 0.5)]
        public double Lower { get; set; }
        
        [Parameter("Risk percentage", Group ="Basic settings", DefaultValue = 2.5)]
        public double RiskPercentage { get; set; }
        
        [Parameter("Atr Multiplier", Group ="Basic settings", DefaultValue = 2)]
        public int AtrMultiplier {get; set;}
        
        [Parameter("Atr Length", Group ="Basic settings", DefaultValue = 20)]
        public int AtrLength { get; set; }
        
        // Filter settings
        [Parameter("Enable Filter", Group ="Filter settings", DefaultValue =false)]
        public bool EnableFilter {get; set;} 
        
        [Parameter("Benchmark Symbol", Group ="Filter settings", DefaultValue ="US500")]
        public string BenchmarkSymbol {get;set;}
        
        protected override void OnStart()
        {   

        }
        
        protected override void OnBar()
        {   
            // **********************************
            // Perform calculations and analysis
            // **********************************
            
            DataSeries symbolClosePrices = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices;
            StandardDeviation std = Indicators.StandardDeviation(symbolClosePrices, Length, MovingAverageType.Simple);
            BollingerBands bollingerBands = Indicators.BollingerBands(symbolClosePrices, Length, std.Result[0], MovingAverageType.Simple);
            
            IndicatorDataSeries topBand = bollingerBands.Top;
            IndicatorDataSeries bottomBand = bollingerBands.Bottom;
            
            string label = $"BollingerBandTrendFollow_cBot-{Symbol.Name}";
             
            // Filter
            DataSeries benchmarkSymbolClosePrices = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices;
            double benchmarkSymbolClose = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices.LastValue;
            bool filter = EnableFilter ? benchmarkSymbolClose >= Indicators.SimpleMovingAverage(benchmarkSymbolClosePrices, 200).Result.LastValue: true;
            
            // Check position
            Position position = Positions.Find(label);
            bool isOpenPosition = position != null;
            
            // Trade amount
            double qty = ((RiskPercentage/100) * Account.Balance) / (AtrMultiplier * Indicators.AverageTrueRange(AtrLength, MovingAverageType.Simple).Result.LastValue);
            double qtyInLots = ((int)(qty /Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
            
            DataSeries close = Bars.ClosePrices;
            
            bool crossOverUpperBand = close.Last(1) < topBand.Last(1) && close.Last(0) > topBand.Last(0);
            bool crossOverLowerBand = close.Last(1) > bottomBand.Last(1) && close.Last(0) < bottomBand.Last(0);
            bool buyCondition = crossOverUpperBand && !isOpenPosition && filter;
            bool sellCondition = crossOverLowerBand && isOpenPosition;
           
            
            // ********************************
            // Manage trade
            // ********************************
            
            // Entry
            if(buyCondition)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, qtyInLots, label);
            }
            
            // Exit
            if(sellCondition) {
                position.Close();
            }
        }
    }
}