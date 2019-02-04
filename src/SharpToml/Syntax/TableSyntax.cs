// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.
namespace SharpToml.Syntax
{
    public sealed class TableSyntax : TableSyntaxBase
    {
        public TableSyntax() : base(SyntaxKind.Table)
        {
        }

        internal override TokenKind OpenTokenKind => TokenKind.OpenBracket;

        internal override TokenKind CloseTokenKind => TokenKind.CloseBracket;
    }
}