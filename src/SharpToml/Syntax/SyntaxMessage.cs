﻿// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.
using System;
using System.Text;

namespace SharpToml.Syntax
{
    public class SyntaxMessage
    {
        public SyntaxMessage(SyntaxMessageKind kind, SourceSpan span, string message)
        {
            Kind = kind;
            Span = span;
            Message = message;
        }

        public SyntaxMessageKind Kind { get; set; }

        public SourceSpan Span { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Span.ToStringSimple());
            builder.Append(" : ");
            switch (Kind)
            {
                case SyntaxMessageKind.Error:
                    builder.Append("error");
                    break;
                case SyntaxMessageKind.Warning:
                    builder.Append("warning");
                    break;
                default:
                    throw new InvalidOperationException($"Message type [{Kind}] not supported");
            }
            builder.Append(" : ");
            if (Message != null)
            {
                builder.Append(Message);
            }
            return builder.ToString();
        }
    }
}