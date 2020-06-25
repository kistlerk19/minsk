﻿ using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.CodeAnalysis;

namespace Minsk
{
    // 1 + 2 * 3
    //
    //
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            bool showTree = false;
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;
                
                if(line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing Parse Trees..." : "Not Showing Parse Tree.");
                    continue;
                }
                else if(line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                // var parser = new Parser(line);
                var syntaxTree = SyntaxTree.Parse(line);

                if(showTree)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ForegroundColor = color;
                }

                if (!syntaxTree.Diagnostics.Any())
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ForegroundColor = color;
                }

            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            
            // └──
            // │
            // ├──

            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if(node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            //indent += "    ";


            indent += isLast ? "   " :  "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }

    }

    


}
