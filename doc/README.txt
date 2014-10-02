This directory contains the basic documentation of Concordion.NET.

With the help of "git subtree" it is published to Github pages (http://concordion.github.io/concordion-net/) as well as the Concordion homepage (http://concordion.org/dotnet/).
The following steps are necessary to publish new updates of this documentation.

---

Initial Setup

+ split branch
Please, use the "subtree" command of git to extract this directory, which contains the documentation, into a new branch:
	$ git subtree split --prefix=doc --branch=gh-pages
	$ git push origin gh-pages

+ pull into web project
To integrate the new gh-pages branch from Concordion.NET into the Concordion website, we need to clone this repository first and then add the Concordion.NET documentation as subtree.
	$ git subtree add --prefix=static-content/dotnet https://github.com/concordion/concordion-net.git gh-pages

---

Update github pages

+ check-in changes to doc of master branch

+ publish: pull changes into gh-pages branch
	$ git subtree push --prefix=doc origin gh-pages

---

Update web page 

+ publish: pull changes into concordion-website
	$ git subtree pull --prefix=static-content/dotnet https://github.com/concordion/concordion-net.git gh-pages