using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Dynamic {
    internal sealed class DynamicRowMetaObject : DynamicMetaObject {
        private static readonly MethodInfo getValueMethod = typeof(IDictionary<string, object>).GetProperty("Item").GetGetMethod();
        private static readonly MethodInfo setValueMethod = typeof(DynamicRow).GetMethod("SetValue", new Type[] { typeof(string), typeof(object) });

        public DynamicRowMetaObject(System.Linq.Expressions.Expression expression, System.Dynamic.BindingRestrictions restrictions) : base(expression, restrictions) { }

        public DynamicRowMetaObject(System.Linq.Expressions.Expression expression, System.Dynamic.BindingRestrictions restrictions, object value) : base(expression, restrictions, value) { }

        private System.Dynamic.DynamicMetaObject CallMethod(MethodInfo method, System.Linq.Expressions.Expression[] parameters) {
            var callMethod = new System.Dynamic.DynamicMetaObject(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Convert(Expression, LimitType), method, parameters), System.Dynamic.BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            return callMethod;
        }

        public override System.Dynamic.DynamicMetaObject BindGetMember(System.Dynamic.GetMemberBinder binder) {
            var parameters = new System.Linq.Expressions.Expression[]
                                 {
                                         System.Linq.Expressions.Expression.Constant(binder.Name)
                                 };

            var callMethod = CallMethod(getValueMethod, parameters);

            return callMethod;
        }

        // Necesario para el soporte en Visual Basic
        public override System.Dynamic.DynamicMetaObject BindInvokeMember(System.Dynamic.InvokeMemberBinder binder, System.Dynamic.DynamicMetaObject[] args) {
            var parameters = new System.Linq.Expressions.Expression[]
                                 {
                                         System.Linq.Expressions.Expression.Constant(binder.Name)
                                 };

            var callMethod = CallMethod(getValueMethod, parameters);

            return callMethod;
        }

        public override System.Dynamic.DynamicMetaObject BindSetMember(System.Dynamic.SetMemberBinder binder, System.Dynamic.DynamicMetaObject value) {
            var parameters = new System.Linq.Expressions.Expression[]
                                 {
                                         System.Linq.Expressions.Expression.Constant(binder.Name),
                                         value.Expression,
                                 };

            var callMethod = CallMethod(setValueMethod, parameters);

            return callMethod;
        }
    }
}
