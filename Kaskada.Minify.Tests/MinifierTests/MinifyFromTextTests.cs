using Kaskada.Minify.Outputs;
using Xunit;

namespace Kaskada.Minify.Tests.MinifierTests
{
    public class MinifyFromTextTests
    {
        [Fact]
        public void Minify_ShouldRemoveSingleLineComment() =>
            AssertResultAsExpected(
                "/* this is a comment */",
                "");

        [Fact]
        public void Minify_ShouldRemoveMultilineComments() =>
            AssertResultAsExpected(
                "/* this is\n * a multiline\n * comment */",
                "");

        [Fact]
        public void Minify_ShouldRemoveCommentInsideBlock() =>
            AssertResultAsExpected(
                "body { /* a comment */ color: red; }",
                "body{color:red}");

        [Fact]
        public void Minify_ShouldRemoveCommentedBlocks() =>
            AssertResultAsExpected(
                "body { /* color: red; */ padding: 10px; }",
                "body{padding:10px}");

        [Fact]
        public void Minify_ShouldNotRemoveNestedComments() =>
            AssertResultAsExpected(
                "body { /* color: /* red; */ blue; */ padding: 10px; }",
                "body{blue;*/padding:10px}");

        [Fact]
        public void Minify_ShouldRemoveNewLineCharacters() =>
            AssertResultAsExpected(
                "\nbody\n{\n color: red; \n}\n\n",
                "body{color:red}");

        [Fact]
        public void Minify_ShouldRemoveLeadingWhitespace() =>
            AssertResultAsExpected(
                "     body { color: red; }",
                "body{color:red}");

        [Fact]
        public void Minify_ShouldRemoveTrailingWhitespace() =>
            AssertResultAsExpected(
                "body { color: red; }         ",
                "body{color:red}");

        [Fact]
        public void Minify_ShouldNotRemoveWhitespaceBeforeDotInSelectors() =>
            AssertResultAsExpected(
                "div .test { color: blue; }",
                "div .test{color:blue}");

        [Fact]
        public void Minify_ShouldNotRemoveWhitespaceBeforeHashInSelectors() =>
            AssertResultAsExpected(
                "div #test { color: blue; }",
                "div #test{color:blue}");

        [Fact]
        public void Minify_ShouldRemoveWhitespaceInSelectors() =>
            AssertResultAsExpected(
                "div > p ~ .test, .test + a #id, #test { color: red; }",
                "div>p~.test,.test+a #id,#test{color:red}");

        [Fact]
        public void Minify_ShouldRemoveWhitespaceInDeclarations() =>
            AssertResultAsExpected(
                "p { color: red; padding: 10px; border: 1px; }",
                "p{color:red;padding:10px;border:1px}");

        [Fact]
        public void Minify_ShouldNotRemoveWhitespaceInMultipartPropertyValue() =>
            AssertResultAsExpected(
                "p { border: 5px solid red; }",
                "p{border:5px solid red}");

        [Fact]
        public void Minify_ShouldNotRemoveWhitespaceInAtRules() =>
            AssertResultAsExpected(
                "@media screen and ( min-width: 900px ){ p { color: red; } } @import \"common.css\" screen;",
                "@media screen and (min-width:900px){p{color:red}}@import \"common.css\" screen;");

        [Fact]
        public void Minify_ShouldRemoveNewLinesInAtRulesSelectors() =>
            AssertResultAsExpected(
                "@media screen and ( min-width: 900px ){\n  p { color: red; } } @import \"common.css\" screen;",
                "@media screen and (min-width:900px){p{color:red}}@import \"common.css\" screen;");

        [Fact]
        public void Minify_ShouldRemoveLastSemicolonInBlock() =>
            AssertResultAsExpected(
                "body { color: red; padding: 10px; }",
                "body{color:red;padding:10px}");

        [Fact]
        public void Minify_ShouldNotRemoveSemicolonInAtRule() =>
        AssertResultAsExpected(
            "@import url('file.css'); @import url('file.css') screen and (orientation: landscape);",
            "@import url('file.css');@import url('file.css')screen and (orientation:landscape);");

        [Fact]
        public void Minify_ShouldNotRemoveWhitespaceBetweenParenthesisAndClassSelector() =>
            AssertResultAsExpected(
                ".foo:not(:hover) .bar {}",
                ".foo:not(:hover) .bar{}");

        [Fact]
        public void Minify_ShouldRemoveWhitespaceBetweenSelectorAndDeclaration() =>
            AssertResultAsExpected(
                ".main .article { background: none repeat scroll 0 0 #c4c54a; }",
                ".main .article{background:none repeat scroll 0 0 #c4c54a}");

        private void AssertResultAsExpected(
            string input,
            string expected)
        {
            Minifier minifier = Minifier.FromText(input);
            TextOutput textOutput = new();

            minifier.Minify(textOutput);
            string actual = textOutput.ToString();

            Assert.Equal(
                expected,
                actual);
        }
    }
}
