Feature: SetPreferredCurrency
    Users of the application are able to change their preferred currency which is nothing more than a default to when
    a new transaction is created. The preferred currency can be changed at any given time.

@Acceptance @MainFlow
Scenario: Set preferred currency
    Given the BE currency
    When I set the preferred currency
    Then the BE currency should be stored

@Acceptance @MainFlow
Scenario: Change preferred currency
    Given the BE currency
    When I set the preferred currency
    And I change the preferred currency to RO
    Then the RO currency should be stored
    And the BE currency should not be stored

@Acceptance @MainFlow
Scenario: Get preferred currency
    Given the stored BE currency
    When I retrieve the preferred currency
    Then it should be the BE currency

@Acceptance @MainFlow
Scenario: Change stored currency
    Given the stored BE currency
    When I change the preferred currency to RO
    And I retrieve the preferred currency
    Then it should be the RO currency

@Acceptance @AlternativeFlow
Scenario: No stored currency
    Given no stored currency
    And RO as current culture
    When I retrieve the preferred currency
    Then it should be the RO currency