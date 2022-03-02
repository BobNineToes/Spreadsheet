// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Yup.", Scope = "type", Target = "~T:ExpressionTree.AdditionNode")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "It really should not be uppercase.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.left")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Checked.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.left")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "If someone else cannot figure this is a node, they shouldn't code.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.left")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "It really should not be uppercase.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.right")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Has to be public this time.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.right")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "If someone else cannot figure this is a node, they shouldn't code.", Scope = "member", Target = "~F:ExpressionTree.BinaryNode.right")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1413:Use trailing comma in multi-line initializers", Justification = "commas should not come after the LAST item in the list.", Scope = "member", Target = "~M:ExpressionTree.ExpTree.PostFix(System.String)~System.String")]
