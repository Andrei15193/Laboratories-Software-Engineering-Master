filter_recipe(Criteria, Recipe) :-
    filter_name(Criteria, Recipe),
    filter_type(Criteria, Recipe),
    filter_preparation_time(Criteria, Recipe),
    filter_ingredients(Criteria, Recipe).

filter_name(Criteria, Recipe) :-
    cs_method(Recipe, 'get_Name', RecipeName),
    cs_method(RecipeName, 'ToLowerInvariant', LowercaseRecipeName),
    cs_method(Criteria, 'get_RecipeName', CriteriaRecipeName),
    cs_method(CriteriaRecipeName, 'ToLowerInvariant', LowercaseCriteriaRecipeName),
    LowercaseRecipeName:'Contains'(LowercaseCriteriaRecipeName, Contains),
    Contains = 'True'.

filter_type(Criteria, Recipe) :-
    cs_method(Recipe, 'get_RecipeType', RecipeType),
    cs_method(Criteria, 'get_RecipeType', CriteriaRecipeType),
    are_equal(RecipeType, CriteriaRecipeType).

are_equal(RecipeType, null).
are_equal(RecipeType, CriteriaRecipeType) :-
    RecipeType == CriteriaRecipeType.

filter_preparation_time(Criteria, Recipe) :-
    cs_method(Recipe, 'get_EstimatedPreparationTimeMinutes', PreparationTime),
    cs_method(Criteria, 'get_MinPreparationTime', MinPreparationTime),
    cs_method(Criteria, 'get_MaxPreparationTime', MaxPreparationTime),
    is_greater_than_or_equal(PreparationTime, MinPreparationTime),
    is_less_than_or_equal(PreparationTime, MaxPreparationTime).

filter_ingredients(Criteria, Recipe) :-
    cs_method(Criteria, 'get_Ingredients', CriteriaIngredients),
    cs_method(CriteriaIngredients, 'GetEnumerator', CriteriaIngredientEnumerator),
    check_all_ingredients(CriteriaIngredientEnumerator, Recipe).

check_all_ingredients(CriteriaIngredientEnumerator, Recipe) :-
    CriteriaIngredientEnumerator:'MoveNext'(HasCurrent),
    check_all_ingredients(CriteriaIngredientEnumerator, Recipe, HasCurrent).
check_all_ingredients(CriteriaIngredientEnumerator, Recipe, 'False').
check_all_ingredients(CriteriaIngredientEnumerator, Recipe, 'True') :-
    cs_method(CriteriaIngredientEnumerator, 'get_Current', CriteriaIngredient),
    check_recipe_ingredients(CriteriaIngredient, Recipe),
    check_all_ingredients(CriteriaIngredientEnumerator, Recipe).

check_recipe_ingredients(CriteriaIngredient, Recipe) :-
    cs_method(CriteriaIngredient, 'get_InclusionOption', InclusionOption),
    InclusionOption:'ToString'(InclusionOptionString),
    check_recipe_ingredients(CriteriaIngredient, Recipe, InclusionOptionString).

check_recipe_ingredients(CriteriaIngredient, Recipe, 'MayContain').

check_recipe_ingredients(CriteriaIngredient, Recipe, 'MustContain') :-
    cs_method(Recipe, 'get_Ingredients', RecipeIngredients),
    cs_method(RecipeIngredients, 'GetEnumerator', RecipeIngredientEnumerator),
    check_any_recipe_ingredients_satistfy(CriteriaIngredient, RecipeIngredientEnumerator).

check_any_recipe_ingredients_satistfy(CriteriaIngredient, RecipeIngredientEnumerator) :-
    RecipeIngredientEnumerator:'MoveNext'(HasCurrent),
    check_any_recipe_ingredients_satistfy(CriteriaIngredient, RecipeIngredientEnumerator, HasCurrent).
check_any_recipe_ingredients_satistfy(CriteriaIngredient, RecipeIngredientEnumerator, 'True') :-
    check_recipe_ingredient_satisfies(CriteriaIngredient, RecipeIngredientEnumerator).

check_recipe_ingredient_satisfies(CriteriaIngredient, RecipeIngredientEnumerator) :-
    cs_method(RecipeIngredientEnumerator, 'get_Current', RecipeIngredient),
    check_ingredient(CriteriaIngredient, RecipeIngredient).
check_recipe_ingredient_satisfies(CriteriaIngredient, RecipeIngredientEnumerator) :-
    check_any_recipe_ingredients_satistfy(CriteriaIngredient, RecipeIngredientEnumerator).

check_ingredient(CriteriaIngredient, RecipeIngredient) :-
    cs_method(CriteriaIngredient, 'get_Name', CriteriaIngredientName),
    cs_method(CriteriaIngredientName, 'ToLowerInvariant', LowercaseCriteriaIngredientName),
    cs_method(RecipeIngredient, 'get_Name', RecipeIngredientName),
    cs_method(RecipeIngredientName, 'ToLowerInvariant', LowercaseRecipeIngredientName),
    LowercaseRecipeIngredientName:'Equals'(LowercaseCriteriaIngredientName, Equals),
    Equals == 'True',
    cs_method(CriteriaIngredient, 'get_MinQuantity', MinQuantity),
    cs_method(CriteriaIngredient, 'get_MaxQuantity', MaxQuantity),
    cs_method(RecipeIngredient, 'get_Quantity', Quantity),
    is_greater_than_or_equal(Quantity, MinQuantity),
    is_less_than_or_equal(Quantity, MaxQuantity).

check_recipe_ingredients(CriteriaIngredient, Recipe, 'MustNotContain') :-
    cs_method(Recipe, 'get_Ingredients', RecipeIngredients),
    cs_method(RecipeIngredients, 'GetEnumerator', RecipeIngredientEnumerator),
    check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator).

check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator) :-
    RecipeIngredientEnumerator:'MoveNext'(HasCurrent),
    check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator, HasCurrent).

check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator, 'False').
check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator, 'True') :-
    cs_method(RecipeIngredientEnumerator, 'get_Current', RecipeIngredient),
    check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient),
    check_all_recipe_ingredients_do_not_satistfy(CriteriaIngredient, RecipeIngredientEnumerator).

check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient) :-
    cs_method(CriteriaIngredient, 'get_Name', CriteriaIngredientName),
    cs_method(CriteriaIngredientName, 'ToLowerInvariant', LowercaseCriteriaIngredientName),
    cs_method(RecipeIngredient, 'get_Name', RecipeIngredientName),
    cs_method(RecipeIngredientName, 'ToLowerInvariant', LowercaseRecipeIngredientName),
    LowercaseRecipeIngredientName:'Equals'(LowercaseCriteriaIngredientName, Equals),
    check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient, Equals).

check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient, 'False').
check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient, 'True') :-
    cs_method(CriteriaIngredient, 'get_MinQuantity', MinQuantity),
    cs_method(CriteriaIngredient, 'get_MaxQuantity', MaxQuantity),
    cs_object(MinQuantity),
    cs_object(MaxQuantity),
    cs_method(RecipeIngredient, 'get_Quantity', Quantity),
    is_less_than(Quantity, MinQuantity).
check_ingredient_does_not_satisfy(CriteriaIngredient, RecipeIngredient, 'True') :-
    cs_method(CriteriaIngredient, 'get_MinQuantity', MinQuantity),
    cs_method(CriteriaIngredient, 'get_MaxQuantity', MaxQuantity),
    cs_object(MinQuantity),
    cs_object(MaxQuantity),
    cs_method(RecipeIngredient, 'get_Quantity', Quantity),
    is_greater_than(Quantity, MaxQuantity).

is_greater_than_or_equal(Value1, null).
is_greater_than_or_equal(Value1, Value2) :-
    Value1:'CompareTo'(Value2, CompareResult),
    CompareResult >= 0.

is_greater_than(Value1, null).
is_greater_than(Value1, Value2) :-
    Value1:'CompareTo'(Value2, CompareResult),
    CompareResult > 0.

is_less_than_or_equal(Value1, null).
is_less_than_or_equal(Value1, Value2) :-
    Value2:'CompareTo'(Value1, CompareResult),
    CompareResult >= 0.

is_less_than(Value1, null).
is_less_than(Value1, Value2) :-
    Value2:'CompareTo'(Value1, CompareResult),
    CompareResult > 0.