This directory contains the basic documentation of Concordion.NET.

With the help of "git subtree" it is published to Github pages (http://concordion.github.io/concordion-net/) as well as the Concordion homepage (http://concordion.org/dotnet/).
The following steps are necessary to publish new updates of this documentation.

---

Initial Setup

+ split branch
[This step only needs to be done once and can be omitted, when you just want to publish updates. It is still described here to provide a complete step-by-step description.]
Please, use the "subtree" command of git to extract this directory, which contains the documentation, into a new branch:
	$ git subtree split --prefix=doc --branch=gh-pages

+ pull into web project
To integrate the new gh-pages branch from Concordion.NET into the Concordion website, we need to clone this repository first and then add the Concordion.NET documentation as subtree.
	$ git clone https://github.com/concordion/concordion-website.git
	$ cd concordion-website
	$ git remote add concordion-net https://github.com/concordion/concordion-net.git
	$ git fetch concordion-net
	$ git subtree add --squash --prefix=static-content/dotnet concordion-net/gh-pages

---

Update github pages

+ check-in changes to doc of master branch

+ publish: pull changes into gh-pages branch

---

Update web page 

+ publish: pull changes into concordion-website