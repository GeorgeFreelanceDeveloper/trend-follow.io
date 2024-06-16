using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None)]
    public class MeanReversalPortfolio_cBot : Robot
    {
        // ********************************
        // User defined inputs
        // ********************************

        // Basic settings
        [Parameter("Enable strategy 1", Group ="Basic settings", DefaultValue = true)]
        public bool EnableStrategy1 { get; set; }
        
        [Parameter("Enable strategy 2", Group ="Basic settings", DefaultValue = true)]
        public bool EnableStrategy2 { get; set; }
        
        [Parameter("Enable strategy 3", Group ="Basic settings", DefaultValue = true)]
        public bool EnableStrategy3 { get; set; }
        
        [Parameter("Enable strategy 4", Group ="Basic settings", DefaultValue = true)]
        public bool EnableStrategy4 { get; set; }
        
        [Parameter("Enable strategy 5", Group ="Basic settings", DefaultValue = true)]
        public bool EnableStrategy5 { get; set; }
        
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
            
            DataSeries highPrices = Bars.HighPrices;
            DataSeries lowPrices = Bars.LowPrices;
            DataSeries closePrices = Bars.ClosePrices;
        
            string label = $"MeanReversalPortfolio_cBot-{Symbol.Name}";
             
            // Filter
            DataSeries benchmarkSymbolClosePrices = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices;
            double benchmarkSymbolClose = benchmarkSymbolClosePrices.LastValue;
            bool filter = EnableFilter ? benchmarkSymbolClose >= Indicators.SimpleMovingAverage(benchmarkSymbolClosePrices, 200).Result.LastValue: true;

            // Check position
            Position position = Positions.Find(label);
            bool isOpenPosition = position != null;

            bool buyCondition = (strategy1(highPrices, lowPrices, closePrices) || strategy2(closePrices) || strategy3(lowPrices, closePrices) || strategy4(lowPrices, highPrices, closePrices) || strategy5(highPrices, lowPrices, closePrices)) && filter && !isOpenPosition;
            bool sellCondition = closePrices.LastValue > highPrices.Last(1) && isOpenPosition;
            
            // Trade amount
            double qty = Account.Balance / closePrices.LastValue;
            double qtyInLots = ((int)(qty /Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
            
            // ********************************
            // Manage trade
            // ********************************
            
            if (buyCondition){
                ExecuteMarketOrder(TradeType.Buy, SymbolName, qtyInLots, label);
            }

            if (sellCondition){
                position.Close();
            }
        }
        
        // ********************************
        // Functions
        // ********************************

        private bool strategy1 (DataSeries highPrices,  DataSeries lowPrices,  DataSeries closePrices){
            
            // Calculate the Average of High minus Low over the Last 25 Days
            double sumHighLowDifferences = 0;
            for (int i = 0; i < 25; i++)
            {
                double high = highPrices.Last(i);
                double low = lowPrices.Last(i);
                sumHighLowDifferences += high - low;
            }
            double averageHighLowDifference = sumHighLowDifferences / 25;

            // Calculate the IBS (Internal Bar Strength) Indicator:
            double ibs = (closePrices.LastValue - lowPrices.LastValue) / (highPrices.LastValue - lowPrices.LastValue);

            // Calculate a band 2.5 times lower than tthe high over the last 10 days by using average from point number 1
            double lowerBand = Bars.HighPrices.Maximum(10) - averageHighLowDifference * 2.5;
            return EnableStrategy1 ? closePrices.LastValue < lowerBand && ibs < 0.3 : false;

        }
        
        private bool strategy2 (DataSeries closePrices){
            DayOfWeek monday = DayOfWeek.Monday;
            //DateTime now = DateTime.Now;
            bool isMonday = Bars.LastBar.OpenTime.DayOfWeek == monday;
            bool isFriday = Bars.Last(1).OpenTime.DayOfWeek == DayOfWeek.Friday;
            bool isThursday = Bars.Last(2).OpenTime.DayOfWeek == DayOfWeek.Thursday;
            bool today_close_lower_than_friday_close = false;
            bool friday_close_lower_than_thursday_close = false;
            
            Print($"isMonday: {isMonday}");
            Print($"isFriday: {isFriday}");
            Print($"isThursday: {isThursday}");
           
            if (isMonday && isFriday && isThursday)
            {
                 today_close_lower_than_friday_close = closePrices.LastValue < closePrices.Last(1);
                 friday_close_lower_than_thursday_close = closePrices.Last(1) < closePrices.Last(2);
                 Print($"today_close_lower_than_friday_close: {today_close_lower_than_friday_close}");
                 Print($"friday_close_lower_than_thursday_close: {friday_close_lower_than_thursday_close}");
                 
            }  
            
            Print($"today_close_lower_than_friday_close: {today_close_lower_than_friday_close}");
            Print($"friday_close_lower_than_thursday_close: {friday_close_lower_than_thursday_close}");
            return EnableStrategy2 ? isMonday && today_close_lower_than_friday_close && friday_close_lower_than_thursday_close : false;
        }
        
        private bool strategy3 (DataSeries lowPrices,  DataSeries closePrices) {
            double lowerChannel = double.MaxValue;
            for (int i = 1; i <= 5; i++)
            {
                 lowerChannel = Math.Min(lowerChannel, lowPrices.Last(i));
            }
            return EnableStrategy3 ? closePrices.LastValue <= lowerChannel : false;
        }
        
        private bool strategy4 (DataSeries lowPrices, DataSeries highPrices, DataSeries closePrices) {
            // Calculate the lowest range of the previous 6 trading days
            double lowestRange = double.MaxValue;
            for (int i = 0; i < 6; i++)
            {
                double range = highPrices.Last(i) - lowPrices.Last(i);
                lowestRange = Math.Min(lowestRange, range);
            }

            // Calculate the 200-day moving average
            double ma200 = Indicators.SimpleMovingAverage(closePrices, 200).Result[0];

            // Entry condition: Today's range is the lowest of the last 6 days AND close is above the 200-day MA
            return EnableStrategy4 ? (lowestRange == highPrices.LastValue - lowPrices.LastValue) && (closePrices.LastValue > ma200) : false;
        }

        private bool strategy5 (DataSeries highPrices, DataSeries lowPrices, DataSeries closePrices){
            // Calculate the highest high of the last ten days
            
            double highestHigh = double.MaxValue;
            for (int i = 1; i <= 10; i++)
            {
                 highestHigh = Math.Max(highestHigh, highPrices.Last(i));
            }

            // Calculate the IBS indicator
            double ibs = (closePrices.LastValue - lowPrices.LastValue) / (highPrices.LastValue - lowPrices.LastValue);

            // Entry condition: Today's high is higher than the previous high of the last ten days AND IBS is below 0.15
            return EnableStrategy5 ? highPrices.LastValue >= highestHigh && ibs < 0.15 : false;
        }
    }
}