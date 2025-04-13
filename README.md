# ElectronicVoting.Admin

**ElectronicVoting.Admin** is the administrative module of a larger electronic voting system currently under active development. This project allows system administrators to manage elections—from setting up voting sessions to monitoring and analyzing their progress.

## Part of a larger system

This project works together with the following components:

- [ElectronicVoting.Validator](https://github.com/Borysiakk/ElectronicVoting.Validator) – responsible for vote validation and security mechanisms.
- [election-manager-frontend](https://github.com/Borysiakk/election-manager-frontend) – frontend application for election administrators.

## Security

The project uses the [lirisi](https://github.com/zbohm/lirisi) library to support system security, particularly in preventing users from voting more than once. This library helps ensure trusted, tamper-resistant message signing and verification within the system.

## Project status

This project is currently in **active development** and does **not yet offer full functionality**. Some features may not work as expected or are still being implemented.
