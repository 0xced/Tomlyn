﻿// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.
namespace SharpToml.Syntax
{
    public sealed class BooleanValueSyntax : ValueSyntax
    {
        private SyntaxToken _token;
        private bool _value;

        public SyntaxToken Token
        {
            get => _token;
            set
            {
                ParentToThis(ref _token, value, TokenKind.True, TokenKind.False);
                // Update the value accordingly
                if (_token != null)
                {
                    _value = _token.Kind == TokenKind.True;
                }
                else
                {
                    _value = false;
                }
            }
        }

        public bool Value
        {
            get => _value;
            set
            {
                // Update the token kind accordingly
                if (_token != null)
                {
                    _token.Kind = value ? TokenKind.True : TokenKind.False;
                }
                else
                {
                    value = false;
                }
                _value = value;
            }
        }

        public override void Visit(ISyntaxVisitor visitor)
        {
            visitor.Accept(this);
        }

        public override int ChildrenCount => 1;

        protected override SyntaxNode GetChildrenImpl(int index)
        {
            return Token;
        }
    }
}