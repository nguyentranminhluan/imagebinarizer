# **ImageBinarizer**  [![.NET Core Desktop](https://github.com/UniversityOfAppliedSciencesFrankfurt/imagebinarizer/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/UniversityOfAppliedSciencesFrankfurt/imagebinarizer/actions/workflows/dotnet-desktop.yml)
Image binarization is the process of taking an image and convert it into black and white image. This process uses thresholds of RBG or Gray to assign black (0) or white (1) to a pixel. Binarization is usually use when trying to extract object from picture.

The purpose of **ImageBinarizer** is to performs binarization of images. It converts a color image to the 2D representation of 01 bits (with 0 and 1). Binarized image is then saved at the provided path with the given name in output path argument:
~~~ shell
--output-image <your image output path>
~~~
The following command gives information for the arguments and how to use them when running **ImageBinarizer**.
~~~ shell
imgbin --help
~~~
## Example:

If you want to create the binrized representation of the same image, execute following command:

~~~ shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)>
~~~

This command will create the same size image in binarized form by using threshold values for each color that equal to average values of them, and then save it to the given folder with the provided filename in "--output-image" argument. Below images are some examples:

<img src="/images/flower.png" width="400">   <img src="/images/Car.png" width="400">
<img src="/images/house.png" width="800">

### Resize
#### Only width or height is provided.
This is the original logo of DAENET:

![](/images/daenet.png) 


If you want to change one of the sides of the image to get a specific width or height (that fit to some frame), the other side will be calculated base on the aspect ratio of the original image. You can try following command:

``` shell
imgbin --input-image <your image input path (c:\path\DAENET.png)> --output-image <your image output path (c:\path\output.txt)> --imagewidth 120
```
or
~~~ shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)> --imageheight 120
~~~

The Binarized image is generated as below with a custom width of 120, height is automatically calculated when using the DAENET logo as the input image:
```
111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111100111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111110001111111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111110000111111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000001111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000000111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000001111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000001111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000011111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000001111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000011111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000001111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000011111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000000111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000000011111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000000001111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000000000011111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000000000000000000000000000111111111111111111111111111111111111111111111111111111111101
111111111111111111111111111111111000000000000000000000000000011111111111111111111111111111111111111111111111111111110001
111111111111111111111111111111110000000000000000000000000000000111111111111111111111111111111111111111111111111111000001
111111111111111111111111111111110000000000000110000000000000000011111111111111111111111111111111111111111111111110001001
111111111111111111111111111111100000000000001110000000000000000000111111111111111111111111111111111111111111111100011001
111111111111111111111111111111100000000000001110000000000000000000000000001111110000000000011111111000000011110000110000
111111111111111111111111111111000000000000001110000000000000000000000000000011110000000000000111110000000001110000110000
111111111111111111111111111110000000011111001100000001111110000000000111110001110011001111100111000011111000110011111100
111111111111111111111111111110000000111111111100000011111111000000011111111001100011111111110011001111111100100111111100
111111111111111111111111111100000001110000111100000011000011000000111000011100100111100001110010011100001110000001100000
111111111111111111111111111000000001100000111100000000000011000000110000011100100111000001110010011000000110000001100001
111111111111111111111111110000000011100000011100000000001111000001110000001100100111001001100000111000000110010001100111
111111111111111111111111110000000011000000011000000011111111000001111111111100000110001001100100111111111110010011100111
111111111111111111111111100000000011000000111000001110000111000001111111111100001110010011100100111111111110010011100111
111111111111111111111111000000000011000000111000001100000111000001100000000000001110010011100100110000000000010011100111
111111111111111111111110000000000011100001111000001100001110000001110000000000001110010011100100111000000000010011000001
111111111111111111111100000000000011110011111000001110011110000000110000111000001100010011001110011100011100110111100001
111111111111111111111000000000000001111111110000001111111110000000011111110000001100000011001110001111111000110011110011
111111111111111111110000000000000000111000000000000111000110000000001111000000000000000000001111000111100001110001110011
111111111111111111100000000000000000000000000000000000000000000000000000000000000000000000001111100000000111111000000011
111111111111111111000000000000000000000000000000000000000000000000000000000000000000000000011111111000001111111100000111
111111111111111110000000000000000000000000000000000000111111111000000000000000000000000000001111111111111111111111111111
111111111111111100000000000000000000000000000011111111111111111111111111000000000000000000000011111111111111111111111111
111111111111111000000000000000000000000011111111111111111111111111111111111110000000000000000000111111111111111111111111
111111111111110000000000000000000001111111111111111111111111111111111111111111111100000000000000011111111111111111111111
111111111111000000000000000000001111111111111111111111111111111111111111111111111111110000000000000111111111111111111111
111111111110000000000000000011111111111111111111111111111111111111111111111111111111111110000000000011111111111111111111
111111111100000000000000111111111111111111111111111111111111111111111111111111111111111111110000000000111111111111111111
111111111000000000000011111111111111111111111111111111111111111111111111111111111111111111111110000000011111111111111111
111111110000000000111111111111111111111111111111111111111111111111111111111111111111111111111111110000000111111111111111
111111000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111100000011111111111111
111110000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111111
111100000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000011111111111
111000001111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111110000111111111
100001111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100001111111
001111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100111111
111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111011111
111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111

```
#### Create image in a custom scale
The resization can be done with a custom of both width and height by executing the following command:
~~~
imgbin --input-image <your image input path (c:\path\DAENET.png)> --output-image <your image output path (c:\path\output.txt)> --imagewidth 120 --imageheight 50
~~~
A binarized image with size of 120x50 is generated as shown below:
~~~
111111111111111111111111111111100111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111110000011111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000000111111111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000000000011111111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000011111111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000011111111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000011111111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111100000000000000000000011111111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111111000000000000000000000000011111111111111111111111111111111111111111111111111111111111111
111111111111111111111111111111110000000000000000000000000000011111111111111111111111111111111111111111111111111111110000
111111111111111111111111111111100000000000000110000000000000000011111111111111111111111111111111111111111111111110000000
111111111111111111111111111111100000000000000110000000000000000000011000001111111000000000011111111110000111111000011000
111111111111111111111111111110000000000000001100000000000000000000000000000001110000000000000011000000000000100000111000
111111111111111111111111111100000000110000111100000010000011000000011000011000100011100001100000001100001100000001110000
111111111111111111111111110000000011100000011000000000000011000000110000001100000011000001100000011000000110000001100001
111111111111111111111111100000000011000000011000000010000111000001111000001000000110000001100000011100000100000001100111
111111111111111111111110000000000011000000110000001100000110000001100000000000000110000001000000011000000000000001000001
111111111111111111111000000000000001110001110000001110001110000000010000110000001100000011000110001100001000000011100001
111111111111111111100000000000000000000000000000000000000000000000000000000000000000000000001111000000000001110000000001
111111111111111110000000000000000000000000000000000000000000000000000000000000000000000000001111111100111111111111111111
111111111111110000000000000000000000000000000011111111111111111111111111000000000000000000000001111111111111111111111111
111111111111000000000000000000000000111111111111111111111111111111111111111111111100000000000000001111111111111111111111
111111111100000000000000000011111111111111111111111111111111111111111111111111111111111111000000000001111111111111111111
111111100000000000000011111111111111111111111111111111111111111111111111111111111111111111111110000000000111111111111111
111110000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111100000000111111111111
111000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000001111111111
100000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100001111111
111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
~~~

### Inverted Image
Another option to binarize the image is to get the inverse of it. The next command provides required argument (--inv) to perform Inverse Binarization:
~~~shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)> -inv
~~~
The pictures below compare the normal binarized image (left) with the inverted binarized image (right):

<img src="/images/NormalConvert.png" width="400"><img src="/images/InverseConvert.png" width="400">


### Gray Scale
In real life, human eyes have different sensitivities with different colors. According to research, Green, Red, then Blue is the order of the sensitivities of the eyes from high to low to the primary light colors. To the have a better performence in binarization according to human eyes, the image should be converted to Gray Scale before binarizing. NTSC formula is used to convert the image:
~~~
grayValue = 0.299*redValue + 0.587*greenValue + 0.114*blueValue
~~~
To enable gray scale, required argument (--gs) need to be provided as shown in below command:
~~~shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)> -gs
~~~
The picture below illustrates the different between RBG scale binarization (the left one) and Gray Scale binarization (the right one) when using the middle image as input:

<p align="center">
    <img src="/images/grayexample.png" >
</p>

### Thresholds customization
One interesting feature of this project is to customize the thresholds for each primary color when perform binarization to have a better binarized result. Use the following command to setup thresholds' values for RBG colors:
~~~shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)> --redthreshold 100 --greenthreshold 100 --bluethreshold 100
~~~
Customized Gray Threshold can also be provided while performing binarization with the following command:
~~~shell
imgbin --input-image <your image input path (c:\path\inputImage.png)> --output-image <your image output path (c:\path\output.txt)> --gs --greythreshold 100
~~~
The image below shows the differences between Normal Binarization (the left one) and Customized Thresholds Binarization (the right one):
<img src="/images/thresholdCustomize.png">

### Create LogoPrinter.cs file code

**ImageBinarizer** can be used to create a console application logo. Making a logo from this image as an example:

<p align="center">
    <img src="/images/triangle.png" width="300">
</p>

To get the code for printing the logo to console, use following command:
~~~shell
imgbin --input-image <your image input path (c:\path\triangle.png)> --create-code
~~~
The size of logo can also be customized, such as setting the width of the logo to 30 by using below command:
~~~shell
imgbin --input-image <your image input path (c:\path\triangle.png)> --create-code -iw 30
~~~
Below is an example of the code in LogoPrinter.cs that **ImageBinarizer** created, with a custom width of logo is 30:
~~~csharp
using System;

namespace LogoBinarizer
{
    public class LogoPrinter
    {
        private string logo = @"
111111111111111111111111111111
111111111111111111111111111111
111111111111001111111111111111
111111111111001111111111111111
111111111110000111111111111111
111111111100000100111111111111
111111111100110100011111111111
111111111000001000011111111111
111111111000001001001111111111
111111110000000000001111111111
111111110000000100100111111111
111111100100110000000011111111
111111000000000000000011111111
111111000001001001001001111111
111110000000011000000001111111
111110000000011100100100111111
111100100100111110000000011111
111000000000111110000010011111
111001000001111111011000001111
110000000000000000000001001111
110000000000000000000000100111
111100100000000000110110000011
111000000000000000000010000011
111001111111111111111001111111
110000000000000000000001111111
110000000000000000000000111111
111111111111111111111111111111
";

        /// <summary>
        /// Print Logo to console
        /// </summary>
        public void Print()
        {
            Console.WriteLine(logo);
        }
    }
}
~~~
Then, copy this file to your project and use code below to run it:
~~~csharp
LogoBinarizer.LogoPrinter logo = new LogoPrinter();
logo.Print();
~~~
For example, calling it in Main():
~~~csharp
using LogoBinarizer;
using System;
using System.IO;

namespace Testing
{
    class Program
    {     
        static void Main(string[] args)
        {
            LogoPrinter logo = new();
            logo.Print();
        }
    }
   
}
~~~
The picture below is an example logo:

<p align="center">
    <img src="/images/console.png" >
</p>


### Further development
Image Contour Recognition is an interesting subject. It helps users to get the shape of objects to apply to other applications such as object detection in machine learning. Right now, Image Contour Recognition can be done with some tries in thresholds setup and give the result as shown below.

<img src="/images/Contour.png">

For future development, a new function will be created to detect the contour automatically without trying different sets of threshold setup.


