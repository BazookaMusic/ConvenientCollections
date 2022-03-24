# Convenient Collections

## Nuget Package

Find it here:  [BazookaMusic.ConvenientCollections](https://www.nuget.org/packages/BazookaMusic.ConvenientCollections/1.0.0)

## What is this repo for?

It will contain a number of useful collections with either better performance than System counterparts or different performance trade-offs.

## What is the file structure of the repo?

Collections are split by type. For example, lookup collection (dictionary likes) can be found in the lookups folder.
Tests are merged in the TestCollectionsFolder.

## What collections are included now?

### Lookups

1. LazySortedList - An alternative to SortedList without a terribly slow insert. It uses binary search for looking up keys but defers sorting until the collection is searched or sort is called explicitly.
