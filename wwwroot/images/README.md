# Grade A LMS Logo Assets

This directory contains the logo assets for the Grade A LMS application.

## Files

- **logo.psd** - Original Photoshop document file from `C:\Users\seigk\Downloads\logo (1).psd`
- **logo.png** - Main logo in PNG format (48x48px) converted from PSD
- **logo-small.png** - Small version of logo (64x64px) used in navbar
- **favicon.ico** - Website favicon (32x32px) displayed in browser tab

## Usage

### In Views/Layout
The logo is integrated into the main layout file:
- Small logo (`logo-small.png`) appears in the navbar brand
- Favicon (`favicon.ico`) appears in browser tab

### HTML References
```html
<!-- Navbar brand -->
<img src="~/images/logo-small.png" alt="Grade A Logo" />

<!-- Favicon -->
<link rel="icon" type="image/x-icon" href="~/images/favicon.ico">
```

## Technical Details

The original PSD file was converted using ImageMagick:
- ImageMagick extracted multiple layers from the PSD
- Main logo was selected and resized for web use
- All images are optimized for web performance

## File Sizes
- logo.psd: 503KB (original)
- logo.png: 48KB (main)
- logo-small.png: 4KB (navbar)
- favicon.ico: 4KB (tab icon)
