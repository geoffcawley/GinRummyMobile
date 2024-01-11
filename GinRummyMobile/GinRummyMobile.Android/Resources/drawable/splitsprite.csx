using System;
using System.Drawing;
using System.Drawing.Common;
int w = 73;
int h = 97;
Bitmap fulldeck = new Bitmap("decksprite.png");
for(int r=0;r<4;r++) {
    for(int c=0;c<13;c++) {
        int x0 = w * c;
        int y0 = h * r;
        Rectangle cloneRect = new Rectangle(x0,y0,w,h);
        System.Drawing.Imaging.PixelFormat format = fulldeck.PixelFormat;
        Bitmap cloneBitmap = fulldeck.Clone(cloneRect, format);
        int index = (13 * r) + c;
        cloneBitmap.Save($"deck\\{index}.png", ImageFormat.Png);
    }
}