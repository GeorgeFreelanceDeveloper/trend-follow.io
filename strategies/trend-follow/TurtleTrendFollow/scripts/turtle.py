# region imports
from AlgorithmImports import *
# endregion

class TurtleV1(QCAlgorithm):

    def initialize(self):

        # ********************************
        # User defined inputs
        # ********************************

        # Basic settings
        self.entry_length = self.get_parameter("entry_length", 20)
        self.exit_length = self.get_parameter("exit_length", 10)
        self.risk_percentage = self.get_parameter("risk_percentage", 2)
        self.atr_multiplier = self.get_parameter("atr_multiplier", 2)
        self.atr_length = self.get_parameter("atr_lenght", 20)

        # Top stocks for turtle in period 2015-2025: AAPL,NVDA,MSFT,AMZN,META,TSLA,AVGO,LLY,COST,HD
        # Top crypto for turtle in period 2015-2025: BTCUSD, ETHUSD
        self.symbols = self.get_parameter("symbols", "AAPL,MSFT,NVDA").split(",")
        self.market_type = self.get_parameter("market_type", "equity")  # equity, crypto

        # Filter settings
        self.enable_filter = True if (self.get_parameter("enable_filter", "False") == "True") else False

        if self.market_type == "equity":
            self.benchmark_symbol = self.get_parameter("benchmark_symbol", "SPY")
        elif self.market_type == "crypto":
            self.benchmark_symbol = self.get_parameter("benchmark_symbol", "BTCUSD")
            self.set_benchmark(lambda x: self.securities[self.benchmark_symbol].Price)

        # ********************************
        # Algorithm settings
        # ********************************

        # Basic
        self.set_start_date(2015, 1, 1)
        # self.set_end_date(2025,2,4)
        self.set_cash(10000)
        self.enable_automatic_indicator_warm_up = True

        if self.market_type == "equity":
            self.markets = {symbol: self.add_equity(symbol, Resolution.DAILY, leverage=10) for symbol in self.symbols}
            self.add_equity(self.benchmark_symbol, Resolution.DAILY)
        elif self.market_type == "crypto":
            self.markets = {symbol: self.add_crypto(symbol, Resolution.DAILY, leverage=10) for symbol in self.symbols}
            self.add_crypto(self.benchmark_symbol, Resolution.DAILY)

        # Init indicators
        self.dchs = {symbol: self.dch(symbol, self.entry_length, self.exit_length) for symbol in self.symbols}

        self.atrs = {symbol: self.atr(symbol, self.atr_length) for symbol in self.symbols}

        self.benchmark_sma200 = self.sma(self.benchmark_symbol, 200)

    def on_data(self, data: Slice):
        for symbol in self.symbols:
            dch = self.dchs[symbol]
            atr = self.atrs[symbol]
            self.strategy(data, symbol, dch, atr)

    def strategy(self, data, symbol, dch, atr):
        # **********************************
        # Perform calculations and analysis
        # **********************************

        # Basic
        if symbol not in data.Bars:
            return

        bar = data.Bars[symbol]
        bar_benchmark = data.Bars[self.benchmark_symbol]

        # Trade amount
        quantity = int(((self.risk_percentage / 100) * self.portfolio.cash_book["USD"].amount) / (
                    self.atr_multiplier * atr.current.value))

        # Filter
        filter = bar_benchmark.close > self.benchmark_sma200[1].value if self.enable_filter else True

        buy_condition = bar.close > dch.upper_band[1].value and filter and not self.portfolio[symbol].is_long
        sell_condition = bar.close < dch.lower_band[1].value

        # ********************************
        # Draw outputs
        # ********************************

        # Plot indicator and prices
        # self.Plot("Custom", "Donchian Channel High", self.dch.upper_band[1].value)
        # self.Plot("Custom", "High Price", bar.high)
        # self.Plot("Custom", "Close Price", bar.close)
        # self.Plot("Custom", "Low Price", bar.low)
        # self.Plot("Custom", "Donchian Channel Low", self.dch.lower_band[1].value)

        # ********************************
        # Manage trade
        # ********************************
        if buy_condition:
            self.market_order(symbol, quantity)

        if sell_condition:
            self.liquidate(symbol)
