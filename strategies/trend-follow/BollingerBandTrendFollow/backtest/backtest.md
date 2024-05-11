# BollingerBandTrendFollow backtest
## Markets
* **SPX**: The S&P 500 Index, also known as the Standard & Poor’s 500 Index, is a market-capitalization-weighted index that tracks the performance of approximately 500 leading publicly traded companies in the United States. 

* **NDQ**: The Nasdaq 100 Index (ticker: NDX) is a stock market index that holds 102 stocks representing 101 non-financial sector companies listed on the Nasdaq exchange. It debuted on January 31, 1985, and has evolved to become a barometer for both U.S. mega-cap stock and technology sector performance.

* **DJI**: The Dow Jones Industrial Average (DJIA), often simply referred to as the Dow, is a prominent stock market index that provides insight into the performance of 30 large publicly traded companies in the United States.

* **BTCUSD**: Bitcoin (BTC) is a decentralized cryptocurrency that was first described in a 2008 whitepaper by an individual or group of individuals using the alias Satoshi Nakamoto. Officially launched in January 2009, Bitcoin is a peer-to-peer online currency that allows transactions to happen directly between equal and independent network participants without the need for any intermediary. Bitcoin is digital money that cannot be inflated or manipulated by any individual, company, government, or central bank. Bitcoin is recognized as one of the initial cryptocurrencies to come into use and has inspired the development of thousands of competing projects. There will only ever be 21 million BTC. Bitcoin is highly divisible, with its smallest unit, i.e. 0.000 000 01 BTC, called a "satoshi" or "sat." As bitcoin's value has risen, its easy divisibility has become a key attribute.

* **ETHUSD**: Ethereum (ETH) is a decentralized, open-source blockchain that aims to become a global platform for decentralized applications and strives to enable users worldwide to write and run software resistant to censorship, downtime, and fraud. As the nonprofit Ethereum Foundation puts it, it "Is for more than payments. It's a marketplace of financial services, games, and apps that can't steal your data or censor you." Ethereum is the second-biggest cryptocurrency by market cap after Bitcoin and a decentralized computing platform that can run a wide variety of applications — including a universe of decentralized finance (or DeFi) apps and services. This platform is powered by its native cryptocurrency, Ether, which is used within the Ethereum network for various operations, making it an integral part of the system.

## Interval
- 30+ years
- 1.1.1990 - 5.5.2024

## SPX
**Inputs**
- Basic settings
    - Length 20
    - Upper: 1
    - Lower: 0.5
    - Risk per trade: 2.5%
    - ATR multiplier: 2
    - ATR length: 20

- Filter settings
    - Enable filter: False
    - Benchmark: SPX

- Time settings
    - From date: 1.1.1990
    - To date: 11.5.2024

- Properties
    - Initial capital: 100 000 USD
    - Pyramiding: 0

**Outputs**
- Net profit: 209% (CAGR 6.15%)
- Total count trades: 239 (avg 6-7 trades per year) 
- Percentage profitability: 43.51%
- Profit Factor: 1.95
- Ratio Avg Win / Avg Loss: 2.05
- Max drawdown: 27.57%

**Equity**
![SPX equity](resources/SPX-equity.png)

**Drawdown**
![SPX drawdown](resources/SPX-drawdown.png)

## NDQ
**Inputs**
- Basic settings
    - Length 20
    - Upper: 1
    - Lower: 0.5
    - Risk per trade: 2.5%
    - ATR multiplier: 2
    - ATR length: 20

- Filter settings
    - Enable filter: False
    - Benchmark: SPX

- Time settings
    - From date: 1.1.1990
    - To date: 11.5.2024

- Properties
    - Initial capital: 100 000 USD
    - Pyramiding: 0

**Outputs**
- Net profit: 511% (CAGR: 15%)
- Total count trades: 237 (avg 6.97 trades per year) 
- Percentage profitability: 45%
- Profit Factor: 2.15
- Ratio Avg Win / Avg Loss: 1.973
- Max drawdown: 22.71%

**Equity**
![NDQ equity](resources/NDQ-equity.png)

**Drawdown**
![NDQ drawdown](resources/NDQ-drawdown.png)

## DJI
**Inputs**
- Basic settings
    - Length 20
    - Upper: 1
    - Lower: 0.5
    - Risk per trade: 2.5%
    - ATR multiplier: 2
    - ATR length: 20

- Filter settings
    - Enable filter: False
    - Benchmark: SPX

- Time settings
    - From date: 1.1.1990
    - To date: 11.5.2024

- Properties
    - Initial capital: 100 000 USD
    - Order size: 100% of equity
    - Pyramiding: 0

**Outputs**
- Net profit: 165% (CAGR: 4.85%)
- Total count trades: 237 (avg 6-7 trades per year) 
- Percentage profitability: 40.51%
- Profit Factor: 1.43
- Ratio Avg Win / Avg Loss: 2.1
- Max drawdown: 20.92%

**Equity**
![DJI equity](resources/DJI-equity.png)

**Drawdown**
![DJI drawdown](resources/DJI-drawdown.png)

## BTCUSD
**Inputs**
- Basic settings
    - Length 20
    - Upper: 1
    - Lower: 0.5

- Filter settings
    - Enable filter: True
    - Benchmark: BTCUSD

- Time settings
    - From date: 1.1.2009
    - To date: 5.5.2024

- Properties
    - Initial capital: 100 000 USD
    - Order size: 20% of equity
    - Pyramiding: 0


**Outputs**
- Net profit: 11 443.14% (CAGR: 762.876%)
- Total count trades: 69 (avg 5-6 trades per year) 
- Percentage profitability: 44.93%
- Profit Factor: 3.981
- Ratio Avg Win / Avg Loss: 4.752
- Max drawdown: 19.05%

**Equity**
![BTCUSD equity](resources/BTCUSD-equity.png)

**Drawdown**
![BTCUSD drawdown](resources/BTCUSD-drawdown.png)

## ETHUSD
**Inputs**
- Basic settings
    - Length 20
    - Upper: 1
    - Lower: 0.5

- Filter settings
    - Enable filter: True
    - Benchmark: BTCUSD

- Time settings
    - From date: 1.1.2015
    - To date: 5.5.2024

- Properties
    - Initial capital: 100 000 USD
    - Order size: 20% of equity
    - Pyramiding: 0

**Outputs**
- Net profit: 906.5% (CAGR: 100.72%)
- Total count trades: 39 (avg 4-5 trades per year) 
- Percentage profitability: 43.59%
- Profit Factor: 4.528
- Ratio Avg Win / Avg Loss: 5.86
- Max drawdown: 13.05%

**Equity**
![ETHUSD equity](resources/ETHUSD-equity.png)

**Drawdown**
![ETHUSD drawdown](resources/ETHUSD-drawdown.png)
