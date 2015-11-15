/*
 * @version P# 1.1.3, on Sept 1 2003;  Prolog Cafe 0.44, on November 12 1999
 * @author Mutsunori Banbara (banbara@pascal.seg.kobe-u.ac.jp)
 * @author Naoyuki Tamura    (tamura@kobe-u.ac.jp)
 * Modified by Jonathan Cook (jjc@dcs.ed.ac.uk)
 */
namespace FoodRecipe.Models.Predicates {

using JJC.Psharp.Lang;
using JJC.Psharp.Lang.Resource;
using Predicates = JJC.Psharp.Predicates;
using Resources = JJC.Psharp.Resources;

public class Test_2 : Predicate {
    static internal readonly SymbolTerm f1 = SymbolTerm.MakeSymbol("get_Value", 1);

    public Term arg1, arg2;

    public Test_2(Term a1, Term a2, Predicate cont) {
        arg1 = a1; 
        arg2 = a2; 
        this.cont = cont;
    }

    public Test_2(){}
    public override void setArgument(Term[] args, Predicate cont) {
        arg1 = args[0]; 
        arg2 = args[1]; 
        this.cont = cont;
    }

    public override Predicate exec( Prolog engine ) {
        engine.setB0();
        Term a1, a2, a3;
        a1 = arg1.Dereference();
        a2 = arg2.Dereference();

        Term[] h2 = {a2};
        a3 = new StructureTerm(f1, h2);
        return new Predicates.colon_2(a1, a3, cont);
    }

    public override int arity() { return 2; }

    public override string ToString() {
        return "test(" + arg1 + ", " + arg2 + ")";
    }
}
}

