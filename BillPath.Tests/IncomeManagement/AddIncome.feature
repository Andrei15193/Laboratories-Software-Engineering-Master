Feature: AddIncome
    Users can add incomes in any currency they like, this has an effect on the available cash in that currency.
    
@Acceptance @MainFlow
Scenario: No incomes
    Given no incomes
    Then there should be a total of 0 incomes
    And the total income in RO currency should be 0
    And the available funds in RO currency should be 0

@Acceptance @MainFlow
Scenario: Adding first income
    Given no incomes
    When I add a new income in RO currency with the amount of 10.5 on 2/1/2015 12:40:56 and 'Test' description
    Then there should be a total of 1 incomes
    And the total income in RO currency should be 10.5
    And the available funds in RO currency should be 10.5
    
@Acceptance @MainFlow
Scenario: Adding two incomes with same timestamp
    Given no incomes
    When I add a new income in RO currency with the amount of 10.5 on 2/1/2015 12:40:56 and 'Test1' description
    When I add a new income in RO currency with the amount of 3.2 on 2/1/2015 12:40:56 and 'Test2' description
    Then there should be a total of 2 incomes
    And the total income in RO currency should be 13.7
    And the available funds in RO currency should be 13.7