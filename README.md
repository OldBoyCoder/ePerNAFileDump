# ePerNAFileDump

This utility extracts image files from an installation of the Fiat ePer system.  It has been tested on release 20 of ePer, 
it is known that later releases of ePer hold their images differently, for instance release 84 holds the images in `res` files 
that are actually zip files.

The utility will scan all `NA` files in the ePer installation and write the images out to sub-folders under `c:\Temp`.
It is left as an exercise for the reader to change this if required.

## Background

For release 20 of ePer the images of parts are held in NA files in folder `C:\Program Files (x86)\Fiat\ePER\data\SP.NA.00900.FCTLR`
(assuming a standard installation).

The filenames are the make code concatenated with the catalog code so 'FPK' is the Fiat Barchetta.  Each file contains a
header block follwed by the images in PNG format.  The structure of the NA file is described below.

## Format of an NA file

All integers little-endian unless otherwise specified.

### File header

- 16 bit integer of unknown purpose
- 16 bit integer giving number of entries in the directory
- Array of directory entries (see below for format)
- Blank section
- PNG files concatenated together

### Directory entry

- 16 bit integer looks to be a counter
- 10 bytes of image name
- 32 bit integer offset of main image
- 32 bit integer length of main image
- 32 bit integer offset of thumbnail
- 32 bit integer length of thumbnail
