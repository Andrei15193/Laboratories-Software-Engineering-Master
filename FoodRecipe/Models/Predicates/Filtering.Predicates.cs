/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class AreEqual_2 : Predicate {
    static internal readonly Predicate AreEqual_2_1 = new AreEqual_2_1();
    static internal readonly Predicate AreEqual_2_2 = new AreEqual_2_2();
    static internal readonly Predicate AreEqual_2_sub_1 = new AreEqual_2_sub_1();

    public Term arg1, arg2;

    public AreEqual_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public AreEqual_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(AreEqual_2_1, AreEqual_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "are_equal(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class AreEqual_2_sub_1 : AreEqual_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(AreEqual_2_2);
    }
}

sealed class AreEqual_2_1 : AreEqual_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("null");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a2, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class AreEqual_2_2 : AreEqual_2 {

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        return new dollar_equalityOfTerm_2(a1, a2, cont);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAllIngredients_2 : Predicate {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("MoveNext", 1);

    public Term arg1, arg2;

    public CheckAllIngredients_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckAllIngredients_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a4 = engine.makeVariable();
        Term[] h2 = {a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new CheckAllIngredients_3(a1, a2, a4, cont);
        return new colon_2(a1, a3, p1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_all_ingredients(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAllIngredients_3 : Predicate {
    static internal readonly Predicate CheckAllIngredients_3_1 = new CheckAllIngredients_3_1();
    static internal readonly Predicate CheckAllIngredients_3_2 = new CheckAllIngredients_3_2();
    static internal readonly Predicate CheckAllIngredients_3_sub_1 = new CheckAllIngredients_3_sub_1();

    public Term arg1, arg2, arg3;

    public CheckAllIngredients_3(Term a1, Term a2, Term a3, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        arg3 = a3; 
        this.cont = cont;
    }

    public CheckAllIngredients_3(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        arg3 = args[2]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.aregs[3] = arg3;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(CheckAllIngredients_3_1, CheckAllIngredients_3_sub_1);
    }

    public override int arity() { return 3; }

    public override string ToString() {
        return "check_all_ingredients(" + arg1 + ", " + arg2 + ", " + arg3 + ")";
    }
}

sealed class CheckAllIngredients_3_sub_1 : CheckAllIngredients_3 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(CheckAllIngredients_3_2);
    }
}

sealed class CheckAllIngredients_3_1 : CheckAllIngredients_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("False");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class CheckAllIngredients_3_2 : CheckAllIngredients_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("True");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_Current");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1, p2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        p1 = new CheckAllIngredients_2(a1, a2, cont);
        p2 = new CheckRecipeIngredients_2(a4, a2, p1);
        return new dollar_csMethod_3(a1, s2, a4, p2);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAllRecipeIngredientsDoNotSatistfy_2 : Predicate {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("MoveNext", 1);

    public Term arg1, arg2;

    public CheckAllRecipeIngredientsDoNotSatistfy_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckAllRecipeIngredientsDoNotSatistfy_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a4 = engine.makeVariable();
        Term[] h2 = {a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new CheckAllRecipeIngredientsDoNotSatistfy_3(a1, a2, a4, cont);
        return new colon_2(a2, a3, p1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_all_recipe_ingredients_do_not_satistfy(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAllRecipeIngredientsDoNotSatistfy_3 : Predicate {
    static internal readonly Predicate CheckAllRecipeIngredientsDoNotSatistfy_3_1 = new CheckAllRecipeIngredientsDoNotSatistfy_3_1();
    static internal readonly Predicate CheckAllRecipeIngredientsDoNotSatistfy_3_2 = new CheckAllRecipeIngredientsDoNotSatistfy_3_2();
    static internal readonly Predicate CheckAllRecipeIngredientsDoNotSatistfy_3_sub_1 = new CheckAllRecipeIngredientsDoNotSatistfy_3_sub_1();

    public Term arg1, arg2, arg3;

    public CheckAllRecipeIngredientsDoNotSatistfy_3(Term a1, Term a2, Term a3, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        arg3 = a3; 
        this.cont = cont;
    }

    public CheckAllRecipeIngredientsDoNotSatistfy_3(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        arg3 = args[2]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.aregs[3] = arg3;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(CheckAllRecipeIngredientsDoNotSatistfy_3_1, CheckAllRecipeIngredientsDoNotSatistfy_3_sub_1);
    }

    public override int arity() { return 3; }

    public override string ToString() {
        return "check_all_recipe_ingredients_do_not_satistfy(" + arg1 + ", " + arg2 + ", " + arg3 + ")";
    }
}

sealed class CheckAllRecipeIngredientsDoNotSatistfy_3_sub_1 : CheckAllRecipeIngredientsDoNotSatistfy_3 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(CheckAllRecipeIngredientsDoNotSatistfy_3_2);
    }
}

sealed class CheckAllRecipeIngredientsDoNotSatistfy_3_1 : CheckAllRecipeIngredientsDoNotSatistfy_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("False");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class CheckAllRecipeIngredientsDoNotSatistfy_3_2 : CheckAllRecipeIngredientsDoNotSatistfy_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("True");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_Current");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1, p2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        p1 = new CheckAllRecipeIngredientsDoNotSatistfy_2(a1, a2, cont);
        p2 = new CheckIngredientDoesNotSatisfy_2(a1, a4, p1);
        return new dollar_csMethod_3(a2, s2, a4, p2);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAnyRecipeIngredientsSatistfy_2 : Predicate {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("MoveNext", 1);

    public Term arg1, arg2;

    public CheckAnyRecipeIngredientsSatistfy_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckAnyRecipeIngredientsSatistfy_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a4 = engine.makeVariable();
        Term[] h2 = {a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new CheckAnyRecipeIngredientsSatistfy_3(a1, a2, a4, cont);
        return new colon_2(a2, a3, p1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_any_recipe_ingredients_satistfy(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckAnyRecipeIngredientsSatistfy_3 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("True");

    public Term arg1, arg2, arg3;

    public CheckAnyRecipeIngredientsSatistfy_3(Term a1, Term a2, Term a3, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        arg3 = a3; 
        this.cont = cont;
    }

    public CheckAnyRecipeIngredientsSatistfy_3(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        arg3 = args[2]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();
        a3 = arg3.Dereference();

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        return new CheckRecipeIngredientSatisfies_2(a1, a2, cont);
    }

    public override int arity() { return 3; }

    public override string ToString() {
        return "check_any_recipe_ingredients_satistfy(" + arg1 + ", " + arg2 + ", " + arg3 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckIngredientDoesNotSatisfy_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_Name");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("ToLowerInvariant");
    static internal readonly SymbolTerm f3 = SymbolTerm.MakeSymbol("Equals", 2);

    public Term arg1, arg2;

    public CheckIngredientDoesNotSatisfy_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckIngredientDoesNotSatisfy_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4, a5, a6, a7, a8;
        Predicate p1, p2, p3, p4, p5;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        a6 = engine.makeVariable();
        a8 = engine.makeVariable();
        Term[] h4 = {a4, a8};
        a7 = new StructureTerm(f3, h4);
        p1 = new CheckIngredientDoesNotSatisfy_3(a1, a2, a8, cont);
        p2 = new colon_2(a6, a7, p1);
        p3 = new dollar_csMethod_3(a5, s2, a6, p2);
        p4 = new dollar_csMethod_3(a2, s1, a5, p3);
        p5 = new dollar_csMethod_3(a3, s2, a4, p4);
        return new dollar_csMethod_3(a1, s1, a3, p5);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_ingredient_does_not_satisfy(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckIngredientDoesNotSatisfy_3 : Predicate {
    static internal readonly Predicate CheckIngredientDoesNotSatisfy_3_1 = new CheckIngredientDoesNotSatisfy_3_1();
    static internal readonly Predicate CheckIngredientDoesNotSatisfy_3_2 = new CheckIngredientDoesNotSatisfy_3_2();
    static internal readonly Predicate CheckIngredientDoesNotSatisfy_3_3 = new CheckIngredientDoesNotSatisfy_3_3();
    static internal readonly Predicate CheckIngredientDoesNotSatisfy_3_sub_1 = new CheckIngredientDoesNotSatisfy_3_sub_1();

    public Term arg1, arg2, arg3;

    public CheckIngredientDoesNotSatisfy_3(Term a1, Term a2, Term a3, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        arg3 = a3; 
        this.cont = cont;
    }

    public CheckIngredientDoesNotSatisfy_3(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        arg3 = args[2]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.aregs[3] = arg3;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(CheckIngredientDoesNotSatisfy_3_1, CheckIngredientDoesNotSatisfy_3_sub_1);
    }

    public override int arity() { return 3; }

    public override string ToString() {
        return "check_ingredient_does_not_satisfy(" + arg1 + ", " + arg2 + ", " + arg3 + ")";
    }
}

sealed class CheckIngredientDoesNotSatisfy_3_sub_1 : CheckIngredientDoesNotSatisfy_3 {
    static internal readonly Predicate CheckIngredientDoesNotSatisfy_3_sub_2 = new CheckIngredientDoesNotSatisfy_3_sub_2();

    public override Predicate exec( Prolog engine ) {
        return engine.retry(CheckIngredientDoesNotSatisfy_3_2, CheckIngredientDoesNotSatisfy_3_sub_2);
    }
}

sealed class CheckIngredientDoesNotSatisfy_3_sub_2 : CheckIngredientDoesNotSatisfy_3 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(CheckIngredientDoesNotSatisfy_3_3);
    }
}

sealed class CheckIngredientDoesNotSatisfy_3_1 : CheckIngredientDoesNotSatisfy_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("False");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class CheckIngredientDoesNotSatisfy_3_2 : CheckIngredientDoesNotSatisfy_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("True");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_MinQuantity");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("get_MaxQuantity");
    static internal readonly SymbolTerm s4 = SymbolTerm.MakeSymbol("get_Quantity");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4, a5, a6;
        Predicate p1, p2, p3, p4, p5;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        a6 = engine.makeVariable();
        p1 = new IsLessThan_2(a6, a4, cont);
        p2 = new dollar_csMethod_3(a2, s4, a6, p1);
        p3 = new dollar_csObject_1(a5, p2);
        p4 = new dollar_csObject_1(a4, p3);
        p5 = new dollar_csMethod_3(a1, s3, a5, p4);
        return new dollar_csMethod_3(a1, s2, a4, p5);
    }
}

sealed class CheckIngredientDoesNotSatisfy_3_3 : CheckIngredientDoesNotSatisfy_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("True");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_MinQuantity");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("get_MaxQuantity");
    static internal readonly SymbolTerm s4 = SymbolTerm.MakeSymbol("get_Quantity");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4, a5, a6;
        Predicate p1, p2, p3, p4, p5;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        a6 = engine.makeVariable();
        p1 = new IsGreaterThan_2(a6, a5, cont);
        p2 = new dollar_csMethod_3(a2, s4, a6, p1);
        p3 = new dollar_csObject_1(a5, p2);
        p4 = new dollar_csObject_1(a4, p3);
        p5 = new dollar_csMethod_3(a1, s3, a5, p4);
        return new dollar_csMethod_3(a1, s2, a4, p5);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckIngredient_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_Name");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("ToLowerInvariant");
    static internal readonly SymbolTerm f3 = SymbolTerm.MakeSymbol("Equals", 2);
    static internal readonly SymbolTerm s5 = SymbolTerm.MakeSymbol("True");
    static internal readonly SymbolTerm s6 = SymbolTerm.MakeSymbol("get_MinQuantity");
    static internal readonly SymbolTerm s7 = SymbolTerm.MakeSymbol("get_MaxQuantity");
    static internal readonly SymbolTerm s8 = SymbolTerm.MakeSymbol("get_Quantity");

    public Term arg1, arg2;

    public CheckIngredient_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckIngredient_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11;
        Predicate p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        a6 = engine.makeVariable();
        a8 = engine.makeVariable();
        Term[] h4 = {a4, a8};
        a7 = new StructureTerm(f3, h4);
        a9 = engine.makeVariable();
        a10 = engine.makeVariable();
        a11 = engine.makeVariable();
        p1 = new IsLessThanOrEqual_2(a11, a10, cont);
        p2 = new IsGreaterThanOrEqual_2(a11, a9, p1);
        p3 = new dollar_csMethod_3(a2, s8, a11, p2);
        p4 = new dollar_csMethod_3(a1, s7, a10, p3);
        p5 = new dollar_csMethod_3(a1, s6, a9, p4);
        p6 = new dollar_equalityOfTerm_2(a8, s5, p5);
        p7 = new colon_2(a6, a7, p6);
        p8 = new dollar_csMethod_3(a5, s2, a6, p7);
        p9 = new dollar_csMethod_3(a2, s1, a5, p8);
        p10 = new dollar_csMethod_3(a3, s2, a4, p9);
        return new dollar_csMethod_3(a1, s1, a3, p10);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_ingredient(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckRecipeIngredientSatisfies_2 : Predicate {
    static internal readonly Predicate CheckRecipeIngredientSatisfies_2_1 = new CheckRecipeIngredientSatisfies_2_1();
    static internal readonly Predicate CheckRecipeIngredientSatisfies_2_2 = new CheckRecipeIngredientSatisfies_2_2();
    static internal readonly Predicate CheckRecipeIngredientSatisfies_2_sub_1 = new CheckRecipeIngredientSatisfies_2_sub_1();

    public Term arg1, arg2;

    public CheckRecipeIngredientSatisfies_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckRecipeIngredientSatisfies_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(CheckRecipeIngredientSatisfies_2_1, CheckRecipeIngredientSatisfies_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_recipe_ingredient_satisfies(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class CheckRecipeIngredientSatisfies_2_sub_1 : CheckRecipeIngredientSatisfies_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(CheckRecipeIngredientSatisfies_2_2);
    }
}

sealed class CheckRecipeIngredientSatisfies_2_1 : CheckRecipeIngredientSatisfies_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_Current");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3;
        Predicate p1;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        a3 = engine.makeVariable();
        p1 = new CheckIngredient_2(a1, a3, cont);
        return new dollar_csMethod_3(a2, s1, a3, p1);
    }
}

sealed class CheckRecipeIngredientSatisfies_2_2 : CheckRecipeIngredientSatisfies_2 {

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        return new CheckAnyRecipeIngredientsSatistfy_2(a1, a2, cont);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckRecipeIngredients_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_InclusionOption");
    static internal readonly SymbolTerm f2 = SymbolTerm.MakeSymbol("ToString", 1);

    public Term arg1, arg2;

    public CheckRecipeIngredients_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public CheckRecipeIngredients_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4, a5;
        Predicate p1, p2;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a5 = engine.makeVariable();
        Term[] h3 = {a5};
        a4 = new StructureTerm(f2, h3);
        p1 = new CheckRecipeIngredients_3(a1, a2, a5, cont);
        p2 = new colon_2(a3, a4, p1);
        return new dollar_csMethod_3(a1, s1, a3, p2);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "check_recipe_ingredients(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class CheckRecipeIngredients_3 : Predicate {
    static internal readonly Predicate CheckRecipeIngredients_3_1 = new CheckRecipeIngredients_3_1();
    static internal readonly Predicate CheckRecipeIngredients_3_2 = new CheckRecipeIngredients_3_2();
    static internal readonly Predicate CheckRecipeIngredients_3_3 = new CheckRecipeIngredients_3_3();
    static internal readonly Predicate CheckRecipeIngredients_3_sub_1 = new CheckRecipeIngredients_3_sub_1();

    public Term arg1, arg2, arg3;

    public CheckRecipeIngredients_3(Term a1, Term a2, Term a3, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        arg3 = a3; 
        this.cont = cont;
    }

    public CheckRecipeIngredients_3(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        arg3 = args[2]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.aregs[3] = arg3;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(CheckRecipeIngredients_3_1, CheckRecipeIngredients_3_sub_1);
    }

    public override int arity() { return 3; }

    public override string ToString() {
        return "check_recipe_ingredients(" + arg1 + ", " + arg2 + ", " + arg3 + ")";
    }
}

sealed class CheckRecipeIngredients_3_sub_1 : CheckRecipeIngredients_3 {
    static internal readonly Predicate CheckRecipeIngredients_3_sub_2 = new CheckRecipeIngredients_3_sub_2();

    public override Predicate exec( Prolog engine ) {
        return engine.retry(CheckRecipeIngredients_3_2, CheckRecipeIngredients_3_sub_2);
    }
}

sealed class CheckRecipeIngredients_3_sub_2 : CheckRecipeIngredients_3 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(CheckRecipeIngredients_3_3);
    }
}

sealed class CheckRecipeIngredients_3_1 : CheckRecipeIngredients_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("MayContain");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class CheckRecipeIngredients_3_2 : CheckRecipeIngredients_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("MustContain");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_Ingredients");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("GetEnumerator");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4, a5;
        Predicate p1, p2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        p1 = new CheckAnyRecipeIngredientsSatistfy_2(a1, a5, cont);
        p2 = new dollar_csMethod_3(a4, s3, a5, p1);
        return new dollar_csMethod_3(a2, s2, a4, p2);
    }
}

sealed class CheckRecipeIngredients_3_3 : CheckRecipeIngredients_3 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("MustNotContain");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_Ingredients");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("GetEnumerator");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4, a5;
        Predicate p1, p2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        a3 = engine.aregs[3].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a3, engine.trail) ) return engine.fail();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        p1 = new CheckAllRecipeIngredientsDoNotSatistfy_2(a1, a5, cont);
        p2 = new dollar_csMethod_3(a4, s3, a5, p1);
        return new dollar_csMethod_3(a2, s2, a4, p2);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class FilterIngredients_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_Ingredients");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("GetEnumerator");

    public Term arg1, arg2;

    public FilterIngredients_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public FilterIngredients_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4;
        Predicate p1, p2;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        p1 = new CheckAllIngredients_2(a4, a2, cont);
        p2 = new dollar_csMethod_3(a3, s2, a4, p1);
        return new dollar_csMethod_3(a1, s1, a3, p2);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "filter_ingredients(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class FilterName_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_Name");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("ToLowerInvariant");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("get_RecipeName");
    static internal readonly SymbolTerm f4 = SymbolTerm.MakeSymbol("Contains", 2);
    static internal readonly SymbolTerm s6 = SymbolTerm.MakeSymbol("True");

    public Term arg1, arg2;

    public FilterName_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public FilterName_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4, a5, a6, a7, a8;
        Predicate p1, p2, p3, p4, p5;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        a6 = engine.makeVariable();
        a8 = engine.makeVariable();
        Term[] h5 = {a6, a8};
        a7 = new StructureTerm(f4, h5);
        p1 = new dollar_unify_2(a8, s6, cont);
        p2 = new colon_2(a4, a7, p1);
        p3 = new dollar_csMethod_3(a5, s2, a6, p2);
        p4 = new dollar_csMethod_3(a1, s3, a5, p3);
        p5 = new dollar_csMethod_3(a3, s2, a4, p4);
        return new dollar_csMethod_3(a2, s1, a3, p5);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "filter_name(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class FilterPreparationTime_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_EstimatedPreparationTimeMinutes");
    static internal readonly SymbolTerm s2 = SymbolTerm.MakeSymbol("get_MinPreparationTime");
    static internal readonly SymbolTerm s3 = SymbolTerm.MakeSymbol("get_MaxPreparationTime");

    public Term arg1, arg2;

    public FilterPreparationTime_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public FilterPreparationTime_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4, a5;
        Predicate p1, p2, p3, p4;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        a5 = engine.makeVariable();
        p1 = new IsLessThanOrEqual_2(a3, a5, cont);
        p2 = new IsGreaterThanOrEqual_2(a3, a4, p1);
        p3 = new dollar_csMethod_3(a1, s3, a5, p2);
        p4 = new dollar_csMethod_3(a1, s2, a4, p3);
        return new dollar_csMethod_3(a2, s1, a3, p4);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "filter_preparation_time(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class FilterRecipe_2 : Predicate {

    public Term arg1, arg2;

    public FilterRecipe_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public FilterRecipe_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2;
        Predicate p1, p2, p3;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        p1 = new FilterIngredients_2(a1, a2, cont);
        p2 = new FilterPreparationTime_2(a1, a2, p1);
        p3 = new FilterType_2(a1, a2, p2);
        return new FilterName_2(a1, a2, p3);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "filter_recipe(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class FilterType_2 : Predicate {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("get_RecipeType");

    public Term arg1, arg2;

    public FilterType_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public FilterType_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3, a4;
        Predicate p1, p2;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        a3 = engine.makeVariable();
        a4 = engine.makeVariable();
        p1 = new AreEqual_2(a3, a4, cont);
        p2 = new dollar_csMethod_3(a1, s1, a4, p1);
        return new dollar_csMethod_3(a2, s1, a3, p2);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "filter_type(" + arg1 + ", " + arg2 + ")";
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class IsGreaterThanOrEqual_2 : Predicate {
    static internal readonly Predicate IsGreaterThanOrEqual_2_1 = new IsGreaterThanOrEqual_2_1();
    static internal readonly Predicate IsGreaterThanOrEqual_2_2 = new IsGreaterThanOrEqual_2_2();
    static internal readonly Predicate IsGreaterThanOrEqual_2_sub_1 = new IsGreaterThanOrEqual_2_sub_1();

    public Term arg1, arg2;

    public IsGreaterThanOrEqual_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public IsGreaterThanOrEqual_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(IsGreaterThanOrEqual_2_1, IsGreaterThanOrEqual_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "is_greater_than_or_equal(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class IsGreaterThanOrEqual_2_sub_1 : IsGreaterThanOrEqual_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(IsGreaterThanOrEqual_2_2);
    }
}

sealed class IsGreaterThanOrEqual_2_1 : IsGreaterThanOrEqual_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("null");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a2, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class IsGreaterThanOrEqual_2_2 : IsGreaterThanOrEqual_2 {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("CompareTo", 2);
    static internal readonly IntegerTerm s3 = new IntegerTerm(0);

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        a4 = engine.makeVariable();
        Term[] h2 = {a2, a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new dollar_greaterOrEqual_2(a4, s3, cont);
        return new colon_2(a1, a3, p1);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class IsGreaterThan_2 : Predicate {
    static internal readonly Predicate IsGreaterThan_2_1 = new IsGreaterThan_2_1();
    static internal readonly Predicate IsGreaterThan_2_2 = new IsGreaterThan_2_2();
    static internal readonly Predicate IsGreaterThan_2_sub_1 = new IsGreaterThan_2_sub_1();

    public Term arg1, arg2;

    public IsGreaterThan_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public IsGreaterThan_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(IsGreaterThan_2_1, IsGreaterThan_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "is_greater_than(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class IsGreaterThan_2_sub_1 : IsGreaterThan_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(IsGreaterThan_2_2);
    }
}

sealed class IsGreaterThan_2_1 : IsGreaterThan_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("null");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a2, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class IsGreaterThan_2_2 : IsGreaterThan_2 {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("CompareTo", 2);
    static internal readonly IntegerTerm s3 = new IntegerTerm(0);

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        a4 = engine.makeVariable();
        Term[] h2 = {a2, a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new dollar_greaterThan_2(a4, s3, cont);
        return new colon_2(a1, a3, p1);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class IsLessThanOrEqual_2 : Predicate {
    static internal readonly Predicate IsLessThanOrEqual_2_1 = new IsLessThanOrEqual_2_1();
    static internal readonly Predicate IsLessThanOrEqual_2_2 = new IsLessThanOrEqual_2_2();
    static internal readonly Predicate IsLessThanOrEqual_2_sub_1 = new IsLessThanOrEqual_2_sub_1();

    public Term arg1, arg2;

    public IsLessThanOrEqual_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public IsLessThanOrEqual_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(IsLessThanOrEqual_2_1, IsLessThanOrEqual_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "is_less_than_or_equal(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class IsLessThanOrEqual_2_sub_1 : IsLessThanOrEqual_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(IsLessThanOrEqual_2_2);
    }
}

sealed class IsLessThanOrEqual_2_1 : IsLessThanOrEqual_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("null");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a2, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class IsLessThanOrEqual_2_2 : IsLessThanOrEqual_2 {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("CompareTo", 2);
    static internal readonly IntegerTerm s3 = new IntegerTerm(0);

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        a4 = engine.makeVariable();
        Term[] h2 = {a1, a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new dollar_greaterOrEqual_2(a4, s3, cont);
        return new colon_2(a2, a3, p1);
    }
}
}

/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class IsLessThan_2 : Predicate {
    static internal readonly Predicate IsLessThan_2_1 = new IsLessThan_2_1();
    static internal readonly Predicate IsLessThan_2_2 = new IsLessThan_2_2();
    static internal readonly Predicate IsLessThan_2_sub_1 = new IsLessThan_2_sub_1();

    public Term arg1, arg2;

    public IsLessThan_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public IsLessThan_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.aregs[1] = arg1;
        engine.aregs[2] = arg2;
        engine.cont = cont;
        return call( engine );
    }

    public virtual Predicate call( Prolog engine ) {
        engine.setB0();
        return engine.jtry(IsLessThan_2_1, IsLessThan_2_sub_1);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "is_less_than(" + arg1 + ", " + arg2 + ")";
    }
}

sealed class IsLessThan_2_sub_1 : IsLessThan_2 {

    public override Predicate exec( Prolog engine ) {
        return engine.trust(IsLessThan_2_2);
    }
}

sealed class IsLessThan_2_1 : IsLessThan_2 {
    static internal readonly SymbolTerm s1 = SymbolTerm.MakeSymbol("null");

    public override Predicate exec( Prolog engine ) {
        Term a1, a2;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        if ( !s1.Unify(a2, engine.trail) ) return engine.fail();
        return cont;
    }
}

sealed class IsLessThan_2_2 : IsLessThan_2 {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("CompareTo", 2);
    static internal readonly IntegerTerm s3 = new IntegerTerm(0);

    public override Predicate exec( Prolog engine ) {
        Term a1, a2, a3, a4;
        Predicate p1;
        a1 = engine.aregs[1].Dereference();
        a2 = engine.aregs[2].Dereference();
        Predicate cont = engine.cont;

        a4 = engine.makeVariable();
        Term[] h2 = {a1, a4};
        a3 = new StructureTerm(f1, h2);
        p1 = new dollar_greaterThan_2(a4, s3, cont);
        return new colon_2(a2, a3, p1);
    }
}
}

