# EXIFy

Exify is console application I created for my personal use because I needed a tool that will help me managing a huge amount of pictures in hundrends of folders on my NAS and will save my time.

The app scans provided directory and finds all direct child folders. 
For each direct child folder it scans all pictures and extracts Date/Time the picutre has been taken (by reading EXIF metadata).
Finally, it renames the directory and prepends the date (range) to the original name in YYYY-MM-DD format (YYYY - year, MM - month, DD- day). This works perfect when sorting directories by name because they are listed in chronological order.

Example: __Vacations__ -> __2018-08-25 Vacations__

If pictures have been taken in a few days then the date range will be prepended (YYYY-MM-DD {start date} - YYYY-MM-DD {end date})

![alt text](https://lh3.googleusercontent.com/UA92sLFWTpL5ltsf9CrL9OCZGfcVXhAvpxGQHbJK-rpTgZfYL5eShyBt6HLz7Y_QfBjMjt_cXYCLiaTiFeJi=w1920-h938)

_Please note that the application by default supports JPG/JPEG formats but you can download the source code and add all extensions you need. (take a look at metadata-extractor to see list of supported types)_

## How to use it?

You can either download the project and compile on your own or just grab Exify.exe file from bin folder and run it.

## Dependencies

* [metadata-extractor](https://github.com/drewnoakes/metadata-extractor) - Extracts Exif, IPTC, XMP, ICC and other metadata from image and video files

## Authors

* **Albert Wajda
