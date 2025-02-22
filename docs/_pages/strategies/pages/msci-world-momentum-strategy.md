---
title: Msci World Momentum strategy
layout: default
permalink: /strategies/pages/msci-world-momentum-strategy/
---

# Msci World Momentum strategy

Momentum-based strategies like this one focus on investing in assets that are showing a strong upward trend. This particular strategy picks 10 large global stocks from index [MSCI World Momentum](https://www.msci.com/indexes/index/703755) and buys them evenly at the beginning of each year. An optional trailing stop helps manage risk by automatically selling when the price drops.

## Key elements of this strategy:

* **Stock selection**: The strategy focuses on 10 large stocks from index [MSCI World Momentum](https://www.msci.com/indexes/index/703755)
* **Portfolio rebalancing**: It takes place once a year (at the beginning of the year, 30 minutes after the market opens). First, all positions are liquidated, then, the capital is divided equally between all 10 stocks.
* **Trailing Stop (optional)**: If enabled, a trailing stop order is added with a certain percentage distance, which helps protect profits when prices fall.

## Code example
xample strategy implementation in Python programming language for trading platform QuantConnect.

```python
# region imports
from AlgorithmImports import *
# endregion

class MsciWorldMomentumV1(QCAlgorithm):

    def initialize(self):

        # ********************************
        # User defined inputs
        # ********************************

        # Basic settings
        self.symbols = self.get_parameter("symbol", "META,AAPL,AVGO,NVDA,JPM,WMT,BRK.B,COST,LLY,NFLX").split(",")
        self.enable_trailing_stop = True if (self.get_parameter("enable_trailing_stop", "False") == "True") else False
        self.trailing_stop_percentage = self.get_parameter("trailing_stop_percentage", 0.3)

        # ********************************
        # Algorithm settings
        # ********************************

        # Basic
        self.set_start_date(2015, 1, 1)
        self.set_cash(10000)
        self.markets = {symbol: self.add_equity(symbol, Resolution.DAILY) for symbol in self.symbols}

        self.Schedule.On(
            self.DateRules.YearStart(self.symbols[0]),
            self.TimeRules.AfterMarketOpen(self.symbols[0], 30),
            self._rebalance_portfolio
        )

    def on_data(self, data: Slice):
        pass

    def _rebalance_portfolio(self):
        self.liquidate()

        for symbol in self.symbols:
            quantity = self.calculate_order_quantity(symbol, 1/len(self.symbols))
            self.market_order(symbol, quantity)
            if self.enable_trailing_stop:
                self.trailing_stop_order(symbol, quantity * -1, self.trailing_stop_percentage, True)
```

* [Source code on GitHub](https://github.com/GeorgeFreelanceDeveloper/trend-follow.io/blob/master/strategies/trend-follow/MsciWorldMomentum/scripts/msci_world_momentum.py)

## Backtests
* [All backtests](../../../backtests/)



