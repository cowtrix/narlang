# narlang

Narlang (**N**arrative **L**anguage) is a markup language for generating documents. 

It was designed as a tool to assist in the composition of long, complex documents. It's purpose is to combine many of the features of a code language with general document production.

## Features (current and planned)

- [x] Single-line comments
- [x] Multi-line comments
- [x] Arbitrary object declaration
- [x] References to other objects
- [x] Object variables
- [x] Render templates
- [x] Markdown Renderer (powered by [Markdig](https://github.com/lunet-io/markdig))
- [x] HTML Renderer
- [ ] VSCode Extension (WIP)
- [ ] Visual Studio Extension

## Core Concepts

### Project Layout

Narlang projects are folders containing `.nls` (narlang source) files. Directories are compiled into an arbitrary number of output documents via the narlang compiler.

### .nls Files

`.nls` files contain all the information required to build the output documents. At minimum, files must define one or more objects that contain data.

### Keywords

`new` - defines a new object. Expects `new <object_type> "<object_name>"`

`document` - defines a new rendering entry point and document output. Each `document` object will generate an output.

`render` - a function that reduces down into a string that is rendered out into the document.

### Object Types

The purpose of object types within narlang is to inform the renderer about how to render the object data. This is done via a templating system. The only special object type is the `document` type. Otherwise, you can arbitrarily declare new or different types.

The narlang compiler comes with some inbuilt templates that you can utilise or modify:

- `chapter` - Formats like a chapter in a book.

- `quote` - Formats like a quote, including support for a `$author` and `$source` formatting.

- `section` - Formats like a section of text, ending with a horizontal rule.

### Compilation

Compile a narlang project like so:

`.\narlangCompiler /input:"<file or directory>" /output:"<directory>" [/format:<string>]

- `input` - An `.nls` file or directory containing `.nls` files
- `output` - An output directory. This directory will be deleted before compilation if it already exists, so be careful.
- `format` - Optional. Specify a renderer. Currently the default is `html` (see: Renderers)

## Examples

This will output a simple document just with the text "Hello World!"

```
new document "Hello World"
{
	render
	{
		Hello World!
	}
}
```

Now, something more complicated. We can reference another object by prepending a `~` character to its identifier (the type followed by the name in double quotation marks) on a new line:

```
new document "Reference Example"
{
	render
	{
		This first line will be output.
		~text "My Text Object"
	}
}

new text "My Text Object"
{
	render
	{
		And then this second line will be output.
	}
}
```

Circular references are not allowed and will cause a compilation error.