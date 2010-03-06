MongoDB-CSharp Framework
========================
MongoDB-CSharp is a library for communicating with MongoDB(http://www.mongodb.org).  
The framework is built on top of MongoDB-CSharp in order to provide static typing 
to the schema-less document-oriented database.

Current Features
----------------
- POCO entities.
  - We don't force your entities into anything.  While you may have to tweak some things
    to allow for persistence, those times should be few and far between.
- Automapping 
  - no need to use xml or create classes for mapping, the framework is able to
    map your objects to and from mongo without any help.
  - There is some robust convention support for tweaking if you desire, and
    almost every option has the ability to be overriden on a class/member basis.
- Linq
  - Linq is fully supported and, while it isn't complete, will get you most of 
    the way there.
- Change Tracking
  - This will allow you to pull down an entity and only persist the changes. 
    It allows you to cut down on bandwidth while not sacrificing features.
- Support for inheritance
  - Inheritance is supported both for collection documents and embedded documents
    through the use of discriminators.
    

Wiki
----
Please see the wiki (http://wiki.github.com/craiggwilson/mongodb-csharp/) for 
help on getting started.