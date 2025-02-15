# region imports
from AlgorithmImports import *
# endregion

class BollingerBandsV1(QCAlgorithm):

    def initialize(self):

        self.length = self.get_parameter("length", 20)
        self.multiplier_upper = self.get_parameter("multiplier_upper", 1)
        self.multiplier_lower = self.get_parameter("multiplier_lower", 0.5)
        self.risk_percentage = self.get_parameter("risk_percentage", 2)
        self.atr_multiplier = self.get_parameter("atr_multiplier", 2)
        self.atr_length = self.get_parameter("atr_lenght", 20)

        self.symbols = self.get_parameter("symbols", "AAPL,MSFT,NVDA").split(",") # BTCUSD,ETHUSD
        self.market_type = self.get_parameter("market_type", "crypto")  # equity, crypto

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
        self.set_cash(10000)
        self.enable_automatic_indicator_warm_up = True

        if self.market_type == "equity":
            self.markets = {symbol: self.add_equity(symbol, Resolution.DAILY, leverage=10) for symbol in self.symbols}
            self.add_equity(self.benchmark_symbol, Resolution.DAILY)
        elif self.market_type == "crypto":
            self.markets = {symbol: self.add_crypto(symbol, Resolution.DAILY, leverage=10) for symbol in self.symbols}
            self.add_crypto(self.benchmark_symbol, Resolution.DAILY)

        # Init indicators
        self.bbs_longer = {symbol: self.bb(symbol, self.length, self.multiplier_upper) for symbol in
                            self.symbols}
        self.bbs_shorter = {symbol: self.bb(symbol, self.length, self.multiplier_lower) for symbol in
                             self.symbols}
        self.atrs = {symbol: self.atr(symbol, self.atr_length) for symbol in self.symbols}
        self.benchmark_sma200 = self.sma(self.benchmark_symbol, 200)

    def on_data(self, data: Slice):
        for symbol in self.symbols:
            bb_longer = self.bbs_longer[symbol]
            bb_shorter = self.bbs_shorter[symbol]
            atr = self.atrs[symbol]
            self.strategy(data, symbol, bb_longer, bb_shorter, atr)

    def strategy(self, data, symbol, bb_longer, bb_shorter, atr):
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

        buy_condition = bar.close > bb_longer.upper_band[1].value and filter and not self.portfolio[symbol].is_long
        sell_condition = bar.close < bb_shorter.lower_band[1].value and self.portfolio[symbol].is_long

        # ********************************
        # Manage trade
        # ********************************
        if buy_condition:
            self.market_order(symbol, quantity)

        if sell_condition:
            self.liquidate(symbol)