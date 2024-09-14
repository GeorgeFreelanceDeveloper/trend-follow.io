---
title: Keltner Channel strategy
layout: page
permalink: /strategies/pages/keltner-channel-strategy/
---
# KeltnerChannelTrendFollow
The purpose of the Keltner Channel Strategy is to use the Keltner Channels to generate trading signals designed to capture significant breakout price moves and capitalize on the resulting trends from these breakouts. This strategy was originally developed by Chester Keltner in the 1960s and later refined by Linda Bradford Raschke in the 1980s

**Table of Contents**
* [About strategy](#about-strategy)
* [Author](#author)
* [Entry and exit conditions for long side](#entry-and-exit-conditions-for-long-side)
* [Filters](#filters)
* [Position sizing](#position-sizing)
* [Management of position](#management-of-position)
* [Code example](#code-example)
* [Backtests](#backtests)
* [Suitable markets for trading](#suitable-markets-for-trading)
* [Resources](#resources)

## About strategy
The Keltner Channel was first introduced by Chester Keltner in the 1960s. The original formula used simple moving averages (SMA) and the high-low price range to calculate the bands. In the 1980s, a new formula was introduced that used average true range (ATR). The ATR method is commonly used today.

In the 1980s, Linda Bradford Raschke introduced significant updates to the indicator, refining its methodology and making it more adaptable to the financial markets. This updated version, credited with greater accuracy and responsiveness, is embraced by traders worldwide.

The Keltner Channel is a volatility-based technical indicator composed of three separate lines. The middle line is an exponential moving average (EMA) of the price. Additional lines are placed above and below the EMA. The upper band is typically set two times the ATR above the EMA, and the lower band is typically set at the inverse of two times the ATR (below the EMA). The bands expand and contract as volatility (measured by ATR) expands and contracts.

Since most price action will be encompassed within the upper and lower bands (the channel), moves outside the channel can signal trend changes or an acceleration of the trend. The direction of the channel, such as up, down, or sideways, can also aid in identifying the trend direction of the asset. 

**Prerequisites**
* Liquidity
* Volatility
* Trending market

<img src="../../../assets/images/keltner_example.png" class="img-fluid">

## Author
Chester Keltner is best known for his contribution to technical analysis in the trading industry, particularly through the development of the **Keltner Channel**. The Keltner Channel is a popular technical indicator used by traders to analyze price trends and volatility in financial markets.

Chester Keltner introduced the Keltner Channel in his 1960 book, **"How to Make Money in Commodities."** While the original formulation of the Keltner Channel used the 10-day moving average of the typical price and a fixed percentage for the channel distance, modern interpretations often use the Exponential Moving Average (EMA) and the ATR for more dynamic calculations.

Keltner’s work laid the foundation for this widely-used tool in the trading industry, and his contributions are still highly regarded by traders and technical analysts today.

## Entry and exit conditions for long side
**Entry**
* Daily close price is above upper band

**Exit**
* Daily close price is below lower band

```pinescript
double multiplierUpper = 1
double multiplierLower = 0.5
ema = ta.ema(source =  close, length = emaLength)
rangema = ta.atr(atrBreakoutLength)
upperChannel = ema + rangema * multiplierUpper
lowerChannel = ema - rangema * multiplierLower

buyCondition =  ta.crossover(close, upperChannel)
sellCondition = ta.crossunder(close, lowerChannel) 
```

## Filters
**Simple**
* Daily close price is above 200 day moving average (bullish environment)

**Advance**

I think using Super trend indicator is more accurate determination of medium-term trend changes from bear market to bull market and vice versa.

* Daily close price is above Super trend indicator(Time frame: Weekly, ATR lenght: 10, Factor: 3)

## Position sizing
The size of the position is determined on the basis of volatility, the more volatile the market, the smaller the positions, and conversely, the less volatile the market, the larger positions are traded so that the risk per trade is always the same in various volatile markets.

**Simple by ATR**
```c#
private double ComputeTradeAmount(){
    int AtrMultiplier = 2;
    double amount = (RiskPerTradeInPercentage * AccountSize) / AtrMultiplier * ATR(20, Days)
    return amount;
}
```

**Advance accurately determine the percentage risk**
```c#
//entryPrice: your entry to market
//stopPrice:  value of lowerband
private double ComputeTradeAmount(double entryPrice, double stopPrice)
{
    double riskPerTrade = (RiskPercentage / 100) * Account.Balance;
    double move = entryPrice - stopPrice;
    double amountRaw = riskPerTrade / ((Math.Abs(move) / Symbol.PipSize) * Symbol.PipValue);
    double amount = ((int)(amountRaw / Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
    return amount;
}
```

## Management of position
- Only one position open for one market.

## Code Example
```c#
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

        [Parameter("Price above SMA(X)", Group ="Filter settings", DefaultValue = 200)]
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
            ExponentialMovingAverage ema = Indicators.ExponentialMovingAverage(Bars.ClosePrices, EmaLength);
            AverageTrueRange atr = Indicators.AverageTrueRange(AtrBreakoutLength, MovingAverageType.Simple);

            double upperChannel = ema.Result.LastValue + atr.Result.LastValue * MultiplierUpper;
            double lowerChannel = ema.Result.LastValue - atr.Result.LastValue * MultiplierLower;

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
```

## Backtests
**Nasdaq 100 index**
Period: 5 years (2019-2024)

<img src="../../../assets/images/keltner_channel_backtest_equity.PNG" class="img-fluid">

<img src="../../../assets/images/keltner_channel_backtest_statistics.PNG" class="img-fluid">

* [All backtests](../../../backtests/)

## Suitable markets for trading
* Cryptocurrencies (Bitcoin, Ethereum)
* Stock indexies (S&P 500, Nasdaq, DJI, NIFTY50)
* Stocks in long-term uptrend (AAPL, MSFT, NVDA, TSLA, AMZN, NFLX, SHOP, MA, ASML, PANW)
* Forex pairs in long-term uptrend (USDTRY, EURTRY, GBPTRY, USDINR, USDCNH) -  <span style="color:red">warning: in reality impossible to trade due to high swap</span>

## Resources
* [keltner-channel - therobusttrader.com](https://therobusttrader.com/keltner-channel/)
* [how-to-day-with-trade-keltner-channels - thebalancemoney.com](https://www.thebalancemoney.com/how-to-day-with-trade-keltner-channels-4051613)
* [keltner-channels - howtotrade.com](https://howtotrade.com/trading-strategies/keltner-channels/)
* [keltnerchannel - investopedia.com](https://www.investopedia.com/terms/k/keltnerchannel.asp)