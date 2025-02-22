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
