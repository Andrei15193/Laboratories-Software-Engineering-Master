using System;
using System.Collections.Generic;
using System.Web.Configuration;
using FoodRecipe.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FoodRecipe.Data
{
    public class AzureTableRecipeRepository
        : IRecipeRepository
    {
        private sealed class RecipeEntity
            : TableEntity
        {
            public RecipeEntity(string recipeName)
                : base(recipeName, recipeName)
            {
                Name = recipeName;
            }
            public RecipeEntity()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public string Description
            {
                get;
                set;
            }

            public string RecipeType
            {
                get;
                set;
            }

            public double EstimatedPreparationTimeMinutes
            {
                get;
                set;
            }
        }
        private sealed class RecipeIngredientEntity
            : TableEntity
        {
            public RecipeIngredientEntity(string id, string recipeName)
                : base(recipeName, id)
            {
                Id = id;
            }
            public RecipeIngredientEntity()
            {
            }

            public string Id
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public double Quantity
            {
                get;
                set;
            }

            public string Unit
            {
                get;
                set;
            }
        }

        private static readonly string _storageConnectionString =
            WebConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;

        internal static void EnsureData()
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var recipeTable = tableClient.GetTableReference("recipe");

            if (recipeTable.CreateIfNotExists())
            {
                var recipeIngredientTable = tableClient.GetTableReference("recipeIngredient");
                recipeIngredientTable.CreateIfNotExists();
                _FillWithData(recipeTable, recipeIngredientTable);
            }
        }
        private static void _FillWithData(CloudTable recipeTable, CloudTable recipeIngredientTable)
        {
            foreach (var recipe in _GetRecipes())
            {
                recipeTable.Execute(TableOperation.Insert(
                    new RecipeEntity(recipe.Name)
                    {
                        Name = recipe.Name,
                        Description = recipe.Description,
                        EstimatedPreparationTimeMinutes = recipe.EstimatedPreparationTimeMinutes,
                        RecipeType = recipe.RecipeType.ToString()
                    }));
                foreach (var recipeIngredient in recipe.Ingredients)
                    recipeIngredientTable.Execute(TableOperation.Insert(
                        new RecipeIngredientEntity(Guid.NewGuid().ToString(), recipe.Name)
                        {
                            Name = recipeIngredient.Name,
                            Quantity = recipeIngredient.Quantity,
                            Unit = recipeIngredient.Unit.ToString()
                        }));
            }
        }
        private static IEnumerable<Recipe> _GetRecipes()
        {
            yield return
                new Recipe
                {
                    Name = "Bourbon Chicken",
                    Description = @"Editor's Note: Named Bourbon Chicken because it was supposedly created by a Chinese cook who worked in a restaurant on Bourbon Street. Heat oil in a large skillet. Add chicken pieces and cook until lightly browned. Remove chicken. Add remaining ingredients, heating over medium Heat until well mixed and dissolved. Add chicken and bring to a hard boil. Reduce heat and simmer for 20 minutes. Serve over hot rice and enjoy.",
                    EstimatedPreparationTimeMinutes = 35,
                    RecipeType = RecipeType.MainCourse,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Boneless chicken breasts",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Lbs
                        },
                        new RecipeIngredient
                        {
                            Name = "Olive oil",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Garlic",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Clove
                        },
                        new RecipeIngredient
                        {
                            Name = "Ginger",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Crushed red pepper flakes",
                            Quantity = 0.75,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Apple juice",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Light brown sugar",
                            Quantity = 0.33,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Ketchup",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Cider vinegar",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Water",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Soy sauce",
                            Quantity = 0.33,
                            Unit = RecipeIngredientUnit.Cups
                        }
                    }
                };
            yield return
                new Recipe
                {
                    Name = "Super Bowl (Or Any Day) Chocolate Cupcakes with Cabernet",
                    Description = @"Preheat oven to 350°F & prepare muffin pans w/paper baking cups. Into the lrg bowl of an electric mixer, sift together the flour, sugar, baking soda, baking powder, salt, cinnamon & cocoa powder. Add oil, vanilla, eggs & wine. Beat w/the electric mixer at low speed for 30 seconds. Turn the mixer speed to high & cont beating for 3 min, scraping the sides occ. Remove the bowl from the mixer & stir in the dried cherries. Pour the batter into the prepared muffin pans & bake for 20 min or till a toothpick inserted comes out clean. To Serve: Spoon a dollop of whipped cream on the center of the completely cooled cupcakes. Add 5-6 bits of dried fruit & a dusting of cinnamon or cocoa powder. NOTE: IMHO dried cranberries may well be a good sub for the dried cherries.",
                    EstimatedPreparationTimeMinutes = 40,
                    RecipeType = RecipeType.Dessert,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "All-purpose flour",
                            Quantity = 1.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Sugar",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Baking soda",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Baking powder",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Salt",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Cinnamon",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Unsweetened cocoa powder",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Vegetable oil",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Vanilla extract",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Eggs",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.None
                        },
                        new RecipeIngredient
                        {
                            Name = "Cabernet sauvignon wine",
                            Quantity = 0.75,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Dried cherries (chopped)",
                            Quantity = 0.75,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Whipped cream (lightly sweetened)",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Cups
                        }
                    }
                };
            yield return
                new Recipe
                {
                    Name = "Margarita Cupcakes With Key Lime Icing",
                    Description = @"In large bowl, combine cake mix, Margarita mix, egg whites and vegetable oil using electric mixer. Stir in lime zest; mix completely. Fill prepared pans 2/3 full. Bake according to box directions (usually 325 degrees - be sure to check box!) 22-24 minutes or until toothpick inserted in center of cupcake comes out clean. Cool in pan on cooling rack 5-8 minutes. Remove cupcakes from pan; cool completely. FOR ICING:. In large bowl, cream butter, cream cheese, juice and zest with electric mixer until light and fluffy.Add 4 cups confectioners' sugar, one cup at a time; continue beating until light and fluffy. If icing is too thin add additional confectioners' sugar 1 tablespoon at a time.",
                    EstimatedPreparationTimeMinutes = 32,
                    RecipeType = RecipeType.Dessert,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Box white cake mix (no pudding in the mix)",
                            Quantity = 4.5,
                            Unit = RecipeIngredientUnit.Ounce
                        },
                        new RecipeIngredient
                        {
                            Name = "Can frozen margarita mix, thawed (undiluted)",
                            Quantity = 10,
                            Unit = RecipeIngredientUnit.Ounce
                        },
                        new RecipeIngredient
                        {
                            Name = "Egg whites",
                            Quantity = 3,
                            Unit = RecipeIngredientUnit.None
                        },
                        new RecipeIngredient
                        {
                            Name = "Vegetable oil",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Grated lime zest",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TableSpoon
                        }
                    }
                };
            yield return
                new Recipe
                {
                    Name = "Panera Broccoli Cheese Soup",
                    Description = @"Sauté onion in butter. Set aside. Cook melted butter and flour using a whisk over medium heat for 3-5 minutes. Stir constantly and add the half & half. Add the chicken stock. Simmer for 20 minutes. Add the broccoli, carrots and onions. Cook over low heat 20-25 minutes. Add salt and pepper. Can be puréed in a blender but I don't. Return to heat and add cheese. Stir in nutmeg. Serve with crusty bread and Enjoy :).",
                    EstimatedPreparationTimeMinutes = 70,
                    RecipeType = RecipeType.Soup,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Melted butter",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Chopped onion",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Medium
                        },
                        new RecipeIngredient
                        {
                            Name = "Melted butter",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Flour",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Half-and-half cream",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Chicken stock",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Fresh broccoli (chopped into bite size pieces)",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Lbs
                        },
                        new RecipeIngredient
                        {
                            Name = "Carrot, julienned",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Nutmeg",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Grated sharp cheddar cheese",
                            Quantity = 8,
                            Unit = RecipeIngredientUnit.Ounce
                        }
                    }
                };
            yield return
                new Recipe
                {
                    Name = "Leighdear's Buffalo Chicken Dip Goes Main Course (Low Carb)",
                    Description = @"Preheat oven to 350°F. Season chicken with salt and pepper. Sauté in oil til browned. Place in greased 13x9 baking dish. Beat the cream cheese with a hand mixer until very smooth, then beat in Ranch dressing and hot sauce. (This is the only way to avoid small lumps of cream cheese). Stir in shredded cheese. Spread over chicken in baking dish. Bake at 350°F for approximately 20-25 minutes.",
                    EstimatedPreparationTimeMinutes = 40,
                    RecipeType = RecipeType.MainCourse,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Boneless skinless chicken breasts (can use thighs, or both)",
                            Quantity = 8,
                            Unit = RecipeIngredientUnit.None
                        },
                        new RecipeIngredient
                        {
                            Name = "Vegetable oil",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.TableSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Cream cheese, softened",
                            Quantity = 3,
                            Unit = RecipeIngredientUnit.Ounce
                        },
                        new RecipeIngredient
                        {
                            Name = "Ranch dressing",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Frank's hot sauce",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "Garlic powder",
                            Quantity = 0.25,
                            Unit = RecipeIngredientUnit.TeaSpoon
                        },
                        new RecipeIngredient
                        {
                            Name = "Shredded cheddar cheese",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        }
                    }
                };
        }

        public IEnumerable<Recipe> GetAll()
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var recipeTable = tableClient.GetTableReference("recipe");
            var recipeIngredientTable = tableClient.GetTableReference("recipeIngredient"); ;

            var recipes = new List<Recipe>();
            foreach (var recipeEntity in recipeTable.CreateQuery<RecipeEntity>().Execute())
                recipes.Add(_GetRecipeFrom(recipeEntity, recipeIngredientTable));

            return recipes;
        }
        private Recipe _GetRecipeFrom(RecipeEntity recipeEntity, CloudTable recipeIngredientTable)
            => new Recipe
            {
                Name = recipeEntity.Name,
                Description = recipeEntity.Description,
                EstimatedPreparationTimeMinutes = recipeEntity.EstimatedPreparationTimeMinutes,
                RecipeType = (RecipeType)Enum.Parse(typeof(RecipeType), recipeEntity.RecipeType),
                Ingredients = _GetRecipeIngredientsFor(recipeEntity, recipeIngredientTable)
            };

        private IEnumerable<RecipeIngredient> _GetRecipeIngredientsFor(RecipeEntity recipeEntity, CloudTable recipeIngredientTable)
        {
            var recipeIngredients = new List<RecipeIngredient>();
            var ingredientEntitiesQuery = new TableQuery<RecipeIngredientEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, recipeEntity.Name));

            foreach (var recipeIngredientEntity in recipeIngredientTable.ExecuteQuery(ingredientEntitiesQuery))
                recipeIngredients.Add(
                    new RecipeIngredient
                    {
                        Name = recipeIngredientEntity.Name,
                        Quantity = recipeIngredientEntity.Quantity,
                        Unit = (RecipeIngredientUnit)Enum.Parse(typeof(RecipeIngredientUnit), recipeIngredientEntity.Unit)
                    });

            return recipeIngredients;
        }
    }
}