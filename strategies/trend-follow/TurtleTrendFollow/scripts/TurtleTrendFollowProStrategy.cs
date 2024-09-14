using System;

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;


namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TurtleTrendFollowPro_cBot : Robot
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
        
        // Filter settings
        [Parameter("Enable Filter", Group ="Filter settings", DefaultValue =false)]
        public bool EnableFilter {get; set;} 
        
        [Parameter("Price above SMA(X)", Group ="Filter settings", DefaultValue =200)]
        public int SmaLength {get;set;}
        
        [Parameter("RSI > X", Group ="Filter settings", DefaultValue = 0)]
        public int RsiValue {get;set;}
        
        
        protected override void OnStart()
        {
            OnBarClosed();
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
            string label = $"TurtleTrendFollow_cBot-{Symbol.Name}-L1";
            Position position = Positions.Find(label);
            
            // Trade amount
            double qtyInLots = ComputeTradeAmount(upperChannel,lowerChannel);
            
            // Filter
            bool priceAboveSMA = closePrice > Indicators.SimpleMovingAverage(Bars.ClosePrices, SmaLength).Result.LastValue;
            bool rsiAboveValue = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14).Result.LastValue > RsiValue;
            bool filter = EnableFilter ? priceAboveSMA && rsiAboveValue : true;
       
            
            // ********************************
            // Manage trade
            // ********************************
            
            if(position == null)
            {
                if(filter)
                {
                
                    // Cancel all pending orders for this symbol
                    foreach(PendingOrder pendingOrder in PendingOrders){
                        if(pendingOrder.Label == label)
                        {
                            CancelPendingOrder(pendingOrder);
                        }
                    }
                
                    // Place stop order or market order
                    double stopLossPips = (Math.Abs(upperChannel - lowerChannel)/Symbol.PipSize);
                    if(closePrice >= upperChannel)
                    {
                      ExecuteMarketOrder(TradeType.Buy, Symbol.Name, qtyInLots, label, stopLossPips, null);
                    } else
                    {
                       PlaceStopOrder(TradeType.Buy,Symbol.Name,qtyInLots,upperChannel,label,stopLossPips,null);
                    }
                }

            } else {
               // Update stop loss
               double stopLossPrice = lowerChannel < position.EntryPrice ? lowerChannel : position.EntryPrice;
               ModifyPosition(position,stopLossPrice,position.TakeProfit);
               
               if(closePrice < lowerChannel){
                    position.Close();
               }
            }

            Print("Sucessful call OnBarClosed() method.");
         }
        
        private double ComputeTradeAmount(double entryPrice, double stopPrice)
        {
            double riskPerTrade = (RiskPercentage / 100) * Account.Balance;
            double move = entryPrice - stopPrice;
            double amountRaw = riskPerTrade / ((Math.Abs(move) / Symbol.PipSize) * Symbol.PipValue);
            double amount = ((int)(amountRaw / Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
            return amount;
        }
        
    }   
}