﻿// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.
namespace SharpToml.Syntax
{
    public abstract class TableEntrySyntax : SyntaxNode
    {
        public override void Visit(ISyntaxVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}