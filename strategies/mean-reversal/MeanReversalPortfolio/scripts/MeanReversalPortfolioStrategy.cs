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
        
            string label = $"BollingerBandTrendFollow_cBot-{Symbol.Name}";
             
            // Filter
            DataSeries benchmarkSymbolClosePrices = MarketData.GetBars(TimeFrame.Daily, BenchmarkSymbol).ClosePrices;
            double benchmarkSymbolClose =benchmarkSymbolClosePrices.LastValue;
            bool filter = EnableFilter ? benchmarkSymbolClose >= Indicators.SimpleMovingAverage(benchmarkSymbolClosePrices, 200).Result.LastValue: true;


            buyCondition = (strategy1() or strategy2() or strategy3() or strategy4() or strategy5()) and filter and tradeDateIsAllowed()
            sellCondition = close > high[1]
            
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

        private void strategy1(PositionOpenedEventArgs args){
        
            // Calculate the Average of High minus Low over the Last 25 Days
            int len = 25;
            double highMinusLow = high-low;
            double avgHighLow = Indicators.SimpleMovingAverage(highMinusLow, len);

            // Calculate the IBS (Internal Bar Strength) Indicator:
            double ibs = (close - low) / (high - low);

            // Calculate a band 2.5 times lower than tthe high over the last 10 days by using average from point number 1
            double lowerBand = ta.highest(10) - avgHighLow * 2.5;
            bool strategy1a = enableStrategy1 ? close < lowerBand and ibs < 0.3 : false;

        }
        
        private void strategy2 (){
            monday = dayofweek == dayofweek.monday
            bool today_close_lower_than_friday_close = close < close[1];
            bool friday_close_lower_than_thursday_close = close[1] < close[2];
            bool strategy2a = enableStrategy2 ? monday and today_close_lower_than_friday_close and friday_close_lower_than_thursday_close : false;
        }
        
        private void strategy3 () {
            lowerChannel = ta.lowest(low[1], 5)
            enableStrategy3 ?  close <= lowerChannel : false
        }
        
        private void strategy4 () {
            // Calculate the lowest range of the previous 6 trading days
            lowestRange = ta.lowest(high - low, 6)

            // Calculate the 200-day moving average
            ma200 = ta.sma(close, 200)

            // Entry condition: Today's range is the lowest of the last 6 days AND close is above the 200-day MA
            enableStrategy4 ? (lowestRange == high - low) and (close > ma200) : false
        }

        private void strategy5(){
            // Calculate the highest high of the last ten days
            highestHigh = ta.highest(high[1], 10)


            // Calculate the IBS indicator
            ibs = (close - low) / (high - low)

            // Entry condition: Today's high is higher than the previous high of the last ten days AND IBS is below 0.15
            enableStrategy5 ? high >= highestHigh and ibs < 0.15 : false
        }
    }
}