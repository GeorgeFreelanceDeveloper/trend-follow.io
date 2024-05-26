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
        
        [Parameter("Benchmark Symbol", Group ="Filter settings", DefaultValue ="US500")]
        public string BenchmarkSymbol {get;set;}
        
        
        
        protected override void OnStart()
        {

        }

        protected override void OnBarClosed()
        {
            // **********************************
            // Perform calculations and analysis
            // **********************************
            
            // Basic
            Print("-----------------------------------------------------------------------------------------");
            double upperChannel = Indicators.DonchianChannel(EntryLength).Top.LastValue;
            Print($"upperChannel:{upperChannel}");
            double lowerChannel = Indicators.DonchianChannel(ExitLength).Bottom.LastValue;
            Print($"lowerChannel:{lowerChannel}");
            double closePrice = Bars.ClosePrices.LastValue;
            Print($"closePrice:{closePrice}");
            string label = $"TurtleTrendFollow_cBot-{Symbol.Name}";
            Print($"label:{label}");
            
            // Trade amount
            double qty = ((RiskPercentage/100) * Account.Balance) / (AtrMultiplier * Indicators.AverageTrueRange(20,MovingAverageType.Simple).Result.LastValue);
            Print($"qty:{qty}");
            double qtyInLots = ((int)(qty /Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
            Print($"qtyInLots:{qtyInLots}");
            
            // Filter
            DataSeries benchmarkSymbolClosePrices = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices;
            Print($"benchmarkSymbolClosePrices:{benchmarkSymbolClosePrices}");
            double benchmarkSymbolClose = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices.LastValue;
            Print($"benchmarkSymbolClose:{benchmarkSymbolClose}");
            bool filter = EnableFilter ? benchmarkSymbolClose >= Indicators.SimpleMovingAverage(benchmarkSymbolClosePrices, 200).Result.LastValue: true;
            Print($"filter:{filter}");
            Position position = Positions.Find(label);
            Print($"position:{position}");
            bool isOpenPosition = position != null;
            Print($"isOpenPosition:{isOpenPosition}");
            
            bool buyCondition = closePrice > upperChannel && !isOpenPosition && filter;
            Print($"buyCondition:{buyCondition}");
            bool sellCondition = closePrice < lowerChannel && isOpenPosition;
            Print($"sellCondition:{sellCondition}");
            
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