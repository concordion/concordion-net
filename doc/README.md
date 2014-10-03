# [Concordion.NET](http://www.concordion.org/dotnet/) Documentation

Welcome to the Concordion.NET documentation pages.

Concordion.NET is a handy, small framework that lets you turn a plain English description of requirements into automated tests. It supports Behavior Driven Development (BDD) as well as Acceptance Test Driven Development (ATDD). Concordion.NET acceptance tests are so readable, they can double up as system documentation. They are written in HTML, so can be easily hyperlinked into a navigable structure. And, since the tests run against the system, you can be confident the documentation is always up-to-date.

To get started, check out the [Overview](http://www.concordion.org/dotnet/index.html) page.

If you are new to Concordion.NET we recommend beginning with the  [Getting Started](http://www.concordion.org/dotnet/GettingStarted.html) guide.

## Publishing Process

The Concordion.NET documentation is maintained together with specifications, source code, and tests in the repository of Concordion.NET (https://github.com/concordion/concordion-net). All documentation pages are grouped under the `doc` directory and published to the Concordion web page (http://www.concordion.net/dotnet/).

The publishing process is composed of two major steps:

1. Making the Concordion.NET documentation available on its own branch.
2. Integrating the documentation branch into the web page of Concordion.

### Initial setup

To publish this Concoridon.NET documentation to the Concordion web page, we need to transfer changes made in the master branch of this Concordion.NET repository to the [concordion-website repository](https://github.com/concordion/concordion-website). This is accomplished by extracting the `doc` directory from the master into the gh-pages branch and by including this gh-pages branch as the sub-directory `static-content/dotnet` into the remote repository of concoridon-website. We are using [git-subtree](https://github.com/git/git/blob/master/contrib/subtree/git-subtree.txt) to move changes between repositories. git-subtree can construct synthetic branches from existing commits and ad remote repositories as sub-directories.

To make this Concordion.NET documentation available on a separate branch, we split this documentation:
```
$ git subtree split --prefix=doc --branch=gh-pages
```
By using the branch ph-pages, this documentation is also published on the project page on GitHub Pages: https://concordion.github.io/concordion-net.

To integrate this Concoridion.NET documentation into the web page of Concordion, we use the following git-subtree command:
```
$ git subtree add --squash --message="initial import of Concordion.NET documentation" --prefix=static-content/dotnet https://github.com/concordion/concordion-net.git gh-pages
```
The `--squash` option ensures that only the documentation is added to the concordion-website without the entire history from the Concordion.NET repository.

### Publish updates

If you want to publish updates of this Concordion.NET documentation to [Concordion.org](http://www.concordion.org/dotnet/), you can use the pull and push commands of git-subtree.

First, push the changes from the master to the gh-pages branch on the [Concordion.NET repository](https://github.com/concordion/concordion-net):
```
$ git subtree push --prefix=doc origin gh-pages
```

Second, pull the ph-pages branch as directory into the [concordion-website repository](https://github.com/concordion/concordion-website):
```
$ git subtree pull --message="<update-comment>" --squash --prefix=static-content/dotnet https://github.com/concordion/concordion-net.git gh-pages
```
Please, provide a commit message for the updates you are publishing.