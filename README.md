# CSS - Simple Database Project
&nbsp;

## Documentation

### Database Manager
The way in which a database is kept is structured in the following manner:

* A database represents a simple directory in which tables are kept
* A table is a named file with the extension .TB in which data is stored
* Columns in table are differentiated by having a ‘!’ symbol in front of them
* Entries in a column are stored in sequence right after their column entry, and are prefixed with an index number. (ex: 0-entry1, 1-entry2)
* All columns and entries have their own line, the ending of a columns entries is marked by either EOF or the beginning of another column.

&nbsp;


### CLIParser
&nbsp;


### Graphic Interface
The Graphic Interface of the database is written using the Windows Presentation Foundation system (or WPF). 
#### Architecture
The architectural pattern used is Model-View-Controller (MVC).
#### How It Works
The WPF interface is written with XAML and C#. The components of the graphic interface (windows, forms) are written with XAML, and the Models and Controllers are written in C#.
