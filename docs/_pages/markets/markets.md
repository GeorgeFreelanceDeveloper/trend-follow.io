---
title: Markets
layout: page
permalink: /markets/
---

# Markets in long-term up trend
_Updated September 14, 2024 (quarterly update)_

The article describes markets in long-term up-trend with strong momentum, that markets are suitable for long-only trend strategies as Turtle, SuperTrend, BollingerBand.
An example of such a markets are the technological stock index Nasdaq 100 or the cryptocurrency Bitcoin. Suitable markets are searched through more than 5000 global markets (stocks, etfs and cryptocurrencies).


**Table of Contents**
* [Benchmarks](#benchmarks)
* [ETFs](#etfs)
* [US stocks](#us-stocks)
* [Australia stocks](#australia-stocks)
* [India stocks](#india-stocks)
* [Argentina stocks](#argentina-stocks)
* [Turkey stocks](#turkey-stocks)
* [Crypto](#crypto)
* [Example markets analyse](#example-markets-analyse)


<img src="../../../assets/images/markets/markets_distribution.png" class="img-fluid">

## Benchmarks
A benchmark is a standard that is used to measure the change in an asset's value or another metric over time. In investing, benchmarks are used as a reference point for the performance of securities, mutual funds, exchange-traded funds, portfolios, or other financial instruments.

* [Benchmarks](/markets/pages/benchmarks/)

## ETFs
Top 30 ETFs for trend following trading from 771 ETFs on US market with Asset Under Management greater than 1 bilion USD.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 2
```

**Result**
<div class="row">
    <div class="col-md-6 mb-5">
        <ul>
            <li>NASDAQ:TQQQ</li>
            <li>AMEX:QLD</li>
            <li>AMEX:UPRO</li>
            <li>AMEX:SPXL</li>
            <li>AMEX:FNGU</li>
            <li>AMEX:IYW</li>
            <li>AMEX:SSO</li>
            <li>AMEX:VGT</li>
            <li>AMEX:XLK</li>
            <li>OTC:ISRCF</li>
            <li>AMEX:IXN</li>
            <li>NASDAQ:QQQ</li>
            <li>AMEX:IWY</li>
            <li>AMEX:SCHG</li>
            <li>AMEX:MGK</li>
            <li>NASDAQ:VONG</li>
            <li>AMEX:IWF</li>
            <li>AMEX:ILCG</li>
        </ul>
    </div>
    <div class="col-md-6 mb-5">
        <ul>
            <li>AMEX:VUG</li>
            <li>AMEX:SPMO</li>
            <li>AMEX:VOOG</li>
            <li>AMEX:IVW</li>
            <li>AMEX:SPYG</li>
            <li>NASDAQ:IUSG</li>
            <li>AMEX:XLG</li>
            <li>OTC:CSTNL</li>
            <li>AMEX:NULG</li>
            <li>AMEX:SPHQ</li>
            <li>AMEX:OEF</li>
            <li>AMEX:IWL</li>
            <li>AMEX:QUAL</li>
            <li>AMEX:MGC</li>
            <li>AMEX:DSI</li>
            <li>AMEX:VOO</li>
            <li>AMEX:SPY</li>
        </ul>
    </div>
</div>

## US stocks
Top 20 stocks for trend following trading from 3000 stocks in index Russell 3000.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**

<div class="row">
    <div class="col-md-6 mb-5">
        <ul>
            <li>NASDAQ:NVDA</li>
            <li>NYSE:FICO</li>
            <li>NYSE:AIV</li>
            <li>NASDAQ:COKE</li>
            <li>NASDAQ:REAX</li>
            <li>NYSE:TPL</li>
            <li>NASDAQ:CSWI</li>
            <li>NASDAQ:AAPL</li>
            <li>NYSE:BOOT</li>
            <li>NASDAQ:HLNE</li>
            <li>NYSE:VRT</li>
        </ul>
    </div>
    <div class="col-md-6 mb-5">
        <ul>
            <li>NASDAQ:CRVL</li>
            <li>NASDAQ:COST</li>
            <li>NASDAQ:VKTX</li>
            <li>NASDAQ:TMDX</li>
            <li>NYSE:AJG</li>
            <li>NASDAQ:VRNS</li>
            <li>NYSE:SPGI</li>
            <li>NASDAQ:BLBD</li>
            <li>NYSE:NEE</li>
            <li>NASDAQ:ASTS</li>
            <li>NASDAQ:OBT</li>
        </ul>
    </div>
</div>

## Australia stocks
Top 20 stocks for trend following trading from 500 stocks in index All Ordinaries.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**
* ASX:WTC
* ASX:PNI
* ASX:MAU
* ASX:TUA
* ASX:REH

**Note**
Currently very few long-term growth markets in medium term up-trend with strong momentum in the Australia exchange.

## India stocks
Top 20 stocks for trend following trading from 500 stocks in index Nifty 500.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**
<div class="row">
    <div class="col-md-6 mb-5">
        <ul>
            <li>NSE:UNOMINDA</li>
            <li>NSE:TRENT</li>
            <li>NSE:JAIBALAJI</li>
            <li>NSE:JSL</li>
            <li>NSE:SAREGAMA</li>
            <li>NSE:TITAGARH</li>
            <li>NSE:RVNL</li>
            <li>NSE:ESCORTS</li>
            <li>NSE:JBMA</li>
            <li>NSE:GPIL</li>
        </ul>
    </div>
    <div class="col-md-6 mb-5">
        <ul>
            <li>NSE:DIXON</li>
            <li>NSE:MAZDOCK</li>
            <li>NSE:BLS</li>
            <li>NSE:RKFORGE</li>
            <li>NSE:COFORGE</li>
            <li>NSE:LINDEINDIA</li>
            <li>NSE:BAJAJFINSV</li>
            <li>NSE:POWERINDIA</li>
            <li>NSE:PERSISTENT</li>
            <li>NSE:POLYMED</li>
        </ul>
    </div>
</div>

## Argentina stocks
Top 20 stocks for trend following trading from 65 stocks on Argentina exchange.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**
<div class="row">
    <div class="col-md-6 mb-5">
        <ul>
            <li>BCBA:TGNO4</li>
            <li>BCBA:CAPX</li>
            <li>BCBA:TGSU2</li>
            <li>BCBA:CGPA2</li>
            <li>BCBA:TRAN</li>
            <li>BCBA:PAMP</li>
            <li>BCBA:AGRO</li>
            <li>BCBA:AUSO</li>
            <li>BCBA:HARG</li>
            <li>BCBA:COME</li>
        </ul>
    </div>
    <div class="col-md-6 mb-5">
        <ul>
            <li>BCBA:MORI</li>
            <li>BCBA:OEST</li>
            <li>BCBA:MIRG</li>
            <li>BCBA:GGAL</li>
            <li>BCBA:METR</li>
            <li>BCBA:BPAT</li>
            <li>BCBA:CEPU</li>
            <li>BCBA:EDN</li>
            <li>BCBA:GRIM</li>
            <li>BCBA:CELU</li>
        </ul>
    </div>
</div>


## Turkey stocks
Top 20 stocks for trend following trading from 562 stocks on Turkey exchange.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**
<div class="row">
    <div class="col-md-6 mb-5">
        <ul>
            <li>BIST:TETMT</li>
            <li>BIST:INTEK</li>
            <li>BIST:RALYH</li>
            <li>BIST:UFUK</li>
            <li>BIST:LINK</li>
            <li>BIST:ORGE</li>
            <li>BIST:METUR</li>
            <li>BIST:QNBFB</li>
            <li>BIST:LYDHO</li>
            <li>BIST:RYGYO</li>
        </ul>
    </div>
    <div class="col-md-6 mb-5">
        <ul>
            <li>BIST:PEHOL</li>
            <li>BIST:BANVT</li>
            <li>BIST:VAKKO</li>
            <li>BIST:ATSYH</li>
            <li>BIST:SELGD</li>
            <li>BIST:BEYAZ</li>
            <li>BIST:BTCIM</li>
            <li>BIST:IZFAS</li>
            <li>BIST:CMENT</li>
            <li>BIST:BSOKE</li>
        </ul>
    </div>
</div>

## Crypto
Top 20 crypto for trend following trading from 800 most traded crypto currencies.

**Query**
```js
filter = longTermGrowth + trend + strategy
sorted = perf10Years desc

longTermGrowth = perfAllTime > 0% + perf10Years > 100% + perf5Years > 50% and perfYearly > 10%
trend = priceAbove200WeeklyMA + priceAbove200DailyMA // long term up-trend and medium term up-trend
strategy = turtleStrategyProfitFactor > 3
```

**Result**
* CRYPTO:TRXUSD
* CRYPTO:GTUSD
* CRYPTO:OMUSD

**Note**
Currently very few long-term growth markets in medium term up-trend with strong momentum in the cryptocurrency market.

## Example markets analyse
* [NVIDIA](/markets/pages/nvda/)
* [LLY](/markets/pages/lly)

