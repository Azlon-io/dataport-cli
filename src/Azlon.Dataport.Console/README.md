# Dataport Console App

A wrapper for clients.
Currently only message type of 'notification' is supported.
Can receive notifications and will download a product xml file.

## deploy

Currently building the cli on other os than Windows doesn't work. It builds, but then gives an runtime error because it can't find the ...Model assembly reference (dll) somehow.
on Windows this implicit reference (dll is part of the client nuget) works.

TODO: fix this incompatibility, and finish the pipeline. option is to move required models to the client project.
https://stackoverflow.com/questions/72984765/nuget-pack-multiple-projects-into-single-nuget-package-with-all-external-depende


