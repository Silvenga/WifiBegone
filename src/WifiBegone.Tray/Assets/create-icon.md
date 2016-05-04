Create multi-sized Windows icon:

```
convert -density 384 {INPUTFILE} -define icon:auto-resize -compress zip -transparent white {OUTPUTFILE}
```