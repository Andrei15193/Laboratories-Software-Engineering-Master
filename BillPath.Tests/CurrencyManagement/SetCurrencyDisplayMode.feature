Feature: SetCurrencyDisplayMode
    Users can set how currencies are displayed, some prefer the ISO code, some the symbol and some want both.

@Acceptance @MainFlow
Scenario: Currency symbol display mode
    Given US currency
    When I display currency with symbol only
    Then the result should be $

@Acceptance @MainFlow
Scenario: Currency ISO code display mode
    Given US currency
    When I display currency with ISO code only
    Then the result should be USD

@Acceptance @MainFlow
Scenario: Currency full display mode
    Given US currency
    When I display currency with symbol and ISO code
    Then the result should be $(USD)