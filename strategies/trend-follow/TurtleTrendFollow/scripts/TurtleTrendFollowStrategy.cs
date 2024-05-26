using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
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
            double upperChannel = Indicators.DonchianChannel(EntryLength).Top.LastValue;
            double lowerChannel = Indicators.DonchianChannel(ExitLength).Bottom.LastValue;
            double closePrice = Bars.ClosePrices.LastValue;
            
            // Trade amount
            double qty = ((RiskPercentage/100) * Account.Balance) / (AtrMultiplier * Indicators.AverageTrueRange(20,MovingAverageType.Simple).Result.LastValue);
            
            // Filter
            double benchmarkSymbolClose = 0; //TODO @LucyFreelanceDeveloper
            bool filter = false; //TODO @LucyFreelanceDeveloper
            bool isOpenPosition = false; //TODO @LucyFreelanceDeveloper
            
            
            bool buyCondition = closePrice > upperChannel && !isOpenPosition && filter;
            bool sellCondition = closePrice < lowerChannel;
            
            // ********************************
            // Manage trade
            // ********************************
            
            // Entry
            if(buyCondition)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, qty);
            }
            
            // Exit
            if(sellCondition)
            {
                //TODO @LucyFreelanceDeveloper close all open positions for current symbol
                ExecuteMarketOrder(TradeType.Sell,SymbolName, qty);
            }            
        }

    }
}