namespace Coulda.Test
{
    using System;
    using System.Linq.Expressions;

    public static class ExpressionExtensions
    {
        public static string PropertyName<OBJECT, PROPERTY>(this Expression<Func<OBJECT, PROPERTY>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        } 

        public static string MethodName<OBJECT,RETURN>(this Expression<Func<OBJECT,RETURN>> expr)
        {
            return ((MethodCallExpression) expr.Body).Method.Name;
        } 
    }
}