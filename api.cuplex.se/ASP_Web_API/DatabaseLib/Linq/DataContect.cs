using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace DatabaseLib.Linq
{
}

namespace DatabaseLib.LinqExtensions
{

    public static class TpLoadOptionsHelper
    {
        // usage example: 
        //	db.LoadOptions = new DataLoadOptions().
        //		TpLoadWith<CostDivision>(cd => cd.Color).
        //		TpLoadWith<CostDivision>(cd => cd.Hotel);
        public static DataLoadOptions TpLoadWith<T>(this DataLoadOptions loadOptions, Expression<Func<T, Object>> expression)
        {
            loadOptions.LoadWith<T>(expression);
            return loadOptions;
        }
        public static DataLoadOptions TpAssociateWith<T>(this DataLoadOptions loadOptions, Expression<Func<T, Object>> expression)
        {
            loadOptions.AssociateWith<T>(expression);
            return loadOptions;
        }
    }


    // from http://www.albahari.com/nutshell/predicatebuilder.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}

