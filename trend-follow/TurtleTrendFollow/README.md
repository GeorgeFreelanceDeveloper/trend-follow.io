# TurtleTrendFollow

## About strategy
The Turtle Trading strategy was developed by Richard Dennis and William Eckhardt in the early 1980s. Story
relates that Richard Dennis, a successful commodity trader, believed that trading could be taught to anyone and decided to conduct an experiment to confirm his theory. He created a group of people known as the "Turtles" and taught them his trade system.

The basic principle of the Turtle Trading strategy is to follow the trend. The goal is to identify and trade strong trends on financial markets. Turtles learned to buy and hold futures contracts for markets that were in a strong uptrend and sell short and hold contracts for markets in a strong downtrend. Personally, I think it's better focus on markets in a long-term up-trend and trade only on the long side.

> "Turtle trading trading system is simple to learn but to work properly
it requires a trending market, confidence in the system, consistency and discipline." Richard Dennis

**Prerequisites**
* Liquidity
* Volatility
* Trending market

![Example](resources/BTCUSD_example.png)

## Author
Richard Dennis, a legendary figure in the world of trading, rose to prominence in the 1980s with his innovative trading strategies. Born in 1949, Dennis started his career as a commodities trader in Chicago. He gained widespread recognition for his role in the Turtle Trading Experiment, where he famously recruited and trained a group of novice traders, teaching them his proprietary trading techniques.

Dennis' trading philosophy emphasized the importance of following systematic rules rather than relying on emotions or intuition. He believed that successful trading could be taught, regardless of one's background or experience. This belief was exemplified in the Turtle experiment, where his students, dubbed the "Turtle Traders," achieved remarkable success.

Richard Dennis's legacy endures as a testament to the power of disciplined trading strategies and the potential for individuals to achieve success in the financial markets with the right guidance and methodology.

## Entry and exit conditions
**Entry**
* Daily close price is above max 20 days high price

**Exit**
* Daily close price is below min 10 days low price

## Filters
**Simple**
* Daily close price is above 200 day moving average (bullish environment)

**Advance**

Using Super trend indicator is more accurate determination of medium-term trend changes from bear market to bull market and vice versa.

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
private double ComputeTradeAmount(double entryPrice, double stopPrice)
{
	double riskPerTrade = (RiskPercentage / 100) * Account.Balance;
        double move = entryPrice - stopPrice;
        double amountRaw = riskPerTrade / ((Math.Abs(move) / Symbol.PipSize) * Symbol.PipValue);
        double amount = ((int)(amountRaw / Symbol.VolumeInUnitsStep)) * Symbol.VolumeInUnitsStep;
        return amount;
}
```

## Management position
- Only one position open for one market.

## Suitable markets for trading
* Cryptocurrencies (Bitcoin, Ethereum)
* Stock indexies (S&P 500, Nasdaq, DJI, NIFTY50)
* Stocks in long-term uptrend (AAPL, MSFT, NVDA, TSLA, AMZN, NFLX, SHOP, MA, ASML, PANW)
* Forex pairs in long-term uptrend (USDTRY, EURTRY, GBPTRY, USDINR, USDCNH) - pozor ale v reálu nemožné obchodovat kvůli vysokému swapu

## Notes

## Resources
* [Kryptoměny – Jak je obchodovat systematicky a vydělávat na růstu i propadu?](https://www.financnik.cz/clanky/obchodni-strategie/kryptomeny-systematicky/#trendove-obchodovani-kryptomen)
* [Jak na Trend Following (trendové obchodování)](https://www.financnik.cz/clanky/obchodni-strategie/trend-following/)
* [Turtle Trading Tutorial](https://www.asktraders.com/learn-to-trade/trading-strategies/turtle-trading-tutorial/)
* [Turtle Trading: History, Strategy & Complete Rules - Analyzing Alpha](https://analyzingalpha.com/turtle-trading)
* [Turtle Trading: A Market Legend](https://www.investopedia.com/articles/trading/08/turtle-trading.asp)
* [TURTLE TRADING - STRATEGY EXPLAINED](https://www.tradingview.com/chart/EURUSD/72x1YqG6-TURTLE-TRADING-STRATEGY-EXPLAINED/)
* [Donchian Channels Formula, Calculations, and Uses](https://www.investopedia.com/terms/d/donchianchannels.asp)
* [Legendy tradingu (3. díl): Experimentátor, který změnil pohled na obchodování](https://www.purple-trading.com/cs/legendy-tradingu-richard-dennis/)
* [Turtles Trading Strategy Explained - COMPREHENSIVE](https://www.youtube.com/watch?v=eotKvzrJVQk)
* [TURTLE TRADERS STRATEGY - The Complete TurtleTrader by Michael Covel. (Richard Dennis)](https://www.youtube.com/watch?v=NJkXSZUHl1g)