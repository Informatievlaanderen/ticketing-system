## [1.17.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.17.2...v1.17.3) (2024-02-19)


### Bug Fixes

* **bump:** add new ECR + change test + prd pipeline ([2cdd647](https://github.com/informatievlaanderen/ticketing-system/commit/2cdd6475ff5862935bc5aac2c9701c7bc0f3eba3))

## [1.17.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.17.1...v1.17.2) (2023-10-19)


### Bug Fixes

* monitoring query ([1f5bee7](https://github.com/informatievlaanderen/ticketing-system/commit/1f5bee7cd0b1d67a63a5cdd351f6c79c1b1947b2))

## [1.17.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.17.0...v1.17.1) (2023-10-18)


### Bug Fixes

* stale ticket notification range logic ([5b6e453](https://github.com/informatievlaanderen/ticketing-system/commit/5b6e453c6530b09abe0952677018cbf6c7c74cd9))

# [1.17.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.16.2...v1.17.0) (2023-10-18)


### Features

* add serilog logging ([7e6506b](https://github.com/informatievlaanderen/ticketing-system/commit/7e6506bc847a877b16a2b0ba97b29d4a53b8f04a))

## [1.16.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.16.1...v1.16.2) (2023-09-13)


### Bug Fixes

* dispose session after executing query ([014d013](https://github.com/informatievlaanderen/ticketing-system/commit/014d013ac23c0cf29c1bd98595ad977f662048b3))
* prevent notificationBackgroundService runs from overlapping ([be27fa0](https://github.com/informatievlaanderen/ticketing-system/commit/be27fa009b9a9985df9011fcda6f47bb17a5d74b))

## [1.16.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.16.0...v1.16.1) (2023-09-01)


### Bug Fixes

* filter on datetime query ([698d00c](https://github.com/informatievlaanderen/ticketing-system/commit/698d00ccb26c61cd63e0b7f41b6d8da9afdfad52))

# [1.16.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.15.1...v1.16.0) (2023-09-01)


### Features

* add registry filtering ([ba653b1](https://github.com/informatievlaanderen/ticketing-system/commit/ba653b1c4d1ac503eeac8dbc4ead15be379f704c))

## [1.15.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.15.0...v1.15.1) (2023-08-28)


### Bug Fixes

* query open tickets ([511e3b7](https://github.com/informatievlaanderen/ticketing-system/commit/511e3b7027454d7362707bea69366f8f187f64e4))
* save created status on create ticket ([40f19c8](https://github.com/informatievlaanderen/ticketing-system/commit/40f19c8c498fa9a0976e1a8aa5a5e9fd32ef2522))

# [1.15.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.14.3...v1.15.0) (2023-08-24)


### Features

* add notification background service ([f1ecf40](https://github.com/informatievlaanderen/ticketing-system/commit/f1ecf409622f8a3dc2b1efbc5347054dcc8cfa24))

## [1.14.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.14.2...v1.14.3) (2023-07-03)


### Bug Fixes

* use parent spancontext on create ticket ([1ec1150](https://github.com/informatievlaanderen/ticketing-system/commit/1ec11504fd3ffc633dafab234cf101495358fa13))

## [1.14.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.14.1...v1.14.2) (2023-07-03)


### Bug Fixes

* created span datadog ([0cafec8](https://github.com/informatievlaanderen/ticketing-system/commit/0cafec8b9fe418aa3281bb2d3171ac95d8856aea))

## [1.14.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.14.0...v1.14.1) (2023-06-30)


### Bug Fixes

* datadog spans + remove getall endpoint ([74166bb](https://github.com/informatievlaanderen/ticketing-system/commit/74166bbbf73993fe0bd2019925b3e3ebbf7ccc60))

# [1.14.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.13.0...v1.14.0) (2023-06-29)


### Bug Fixes

* set max on GetAll ([4d70ff9](https://github.com/informatievlaanderen/ticketing-system/commit/4d70ff942ff2927bc3205053325596e93aab8b0a))


### Features

* add datadog spans ([bbb0037](https://github.com/informatievlaanderen/ticketing-system/commit/bbb00376f9fc2c712fa49a92c094643e99b474ce))

# [1.13.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.12.4...v1.13.0) (2023-06-29)


### Bug Fixes

* all endpoint ([11da174](https://github.com/informatievlaanderen/ticketing-system/commit/11da1748197920dbe13ac9e0045a7d6e45c289b0))


### Features

* add datadog trace ([f1d284f](https://github.com/informatievlaanderen/ticketing-system/commit/f1d284f57fade7940e0899bf2fabd8f711989100))

## [1.12.4](https://github.com/informatievlaanderen/ticketing-system/compare/v1.12.3...v1.12.4) (2023-06-22)


### Bug Fixes

* build.fsx monitoring ([7ebcadf](https://github.com/informatievlaanderen/ticketing-system/commit/7ebcadf102ab5ec73691d64c39984f6aa79b55c0))

## [1.12.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.12.2...v1.12.3) (2023-06-22)


### Bug Fixes

* monitoring init ([08ac5db](https://github.com/informatievlaanderen/ticketing-system/commit/08ac5dbb62022e51a3703a000bc1f2e6f07b9c8b))

## [1.12.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.12.1...v1.12.2) (2023-06-22)


### Bug Fixes

* set file properties to copy always ([50c8c03](https://github.com/informatievlaanderen/ticketing-system/commit/50c8c032d440238d338ccccaaf0dd2106cb39689))

## [1.12.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.12.0...v1.12.1) (2023-06-20)


### Bug Fixes

* style to trigger build ([3ee3c9f](https://github.com/informatievlaanderen/ticketing-system/commit/3ee3c9ff51f37205d32c0e4155462e99a7dfe2a8))

# [1.12.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.11.3...v1.12.0) (2023-06-20)


### Bug Fixes

* correct filter on status query + remove status endpoint ([c7262bb](https://github.com/informatievlaanderen/ticketing-system/commit/c7262bb4406717c99607d1302af0a1ce1de7c2bf))
* release monitoring ([bbf599d](https://github.com/informatievlaanderen/ticketing-system/commit/bbf599dbff8a6d315b3ab4a05fd2828b3735484f))


### Features

* add getall endpoint ([c1dd257](https://github.com/informatievlaanderen/ticketing-system/commit/c1dd257a6413b4d2d4596c1c2c47afd99c7bd04e))

## [1.11.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.11.2...v1.11.3) (2023-05-31)


### Bug Fixes

* http proxy json deserialization ([a4f1892](https://github.com/informatievlaanderen/ticketing-system/commit/a4f1892987f5d7c77a7ebfff28a06c71708059ae))

## [1.11.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.11.1...v1.11.2) (2023-05-10)


### Bug Fixes

* style to trigger build ([9604b50](https://github.com/informatievlaanderen/ticketing-system/commit/9604b50b18195167fe7cb1acb56f62757ccbc180))
* to trigger build add NewProduction CI ([30e8655](https://github.com/informatievlaanderen/ticketing-system/commit/30e8655230260b7eed76a7d6235e962be1de60f5))

## [1.11.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.11.0...v1.11.1) (2023-04-28)


### Bug Fixes

* try metrics another way ([1033c75](https://github.com/informatievlaanderen/ticketing-system/commit/1033c75799223ba8157757af0bfb0b7ef1b3cde7))

# [1.11.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.10.0...v1.11.0) (2023-04-28)


### Features

* first try add a metric ([f87e896](https://github.com/informatievlaanderen/ticketing-system/commit/f87e8962686ea54ee928500f75e7813bdffb73f5))

# [1.10.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.9.3...v1.10.0) (2023-04-28)


### Features

* add elastic apm tracing/apm ([06ae669](https://github.com/informatievlaanderen/ticketing-system/commit/06ae6696970be4535c3754b911989d6ed52e6784))

## [1.9.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.9.2...v1.9.3) (2023-04-25)


### Bug Fixes

* don't write nullable ticketerror properties when null ([8236198](https://github.com/informatievlaanderen/ticketing-system/commit/823619890d26c7bed98ee7c940855f80c38906fc))
* run containers non-root ([e2c8509](https://github.com/informatievlaanderen/ticketing-system/commit/e2c8509bcb9b40a8521104d7b07d61b2c2d9b3ab))

## [1.9.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.9.1...v1.9.2) (2023-04-18)


### Bug Fixes

* Equality for TicketError sequence ([4a283ff](https://github.com/informatievlaanderen/ticketing-system/commit/4a283ff251f3a7ec525ec2122f2bbb48f7b674c1))
* Fixed test method name ([6be9492](https://github.com/informatievlaanderen/ticketing-system/commit/6be9492079d7f9886f429381117cc155d54cd50e))
* TicketError equality checks ([1b9ad37](https://github.com/informatievlaanderen/ticketing-system/commit/1b9ad3768a7208fa3a9da9d1df4a14ad3d512621))

## [1.9.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.9.0...v1.9.1) (2023-04-17)

# [1.9.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.8.1...v1.9.0) (2023-04-13)


### Features

* add support for multiple errors ([fc8ab7a](https://github.com/informatievlaanderen/ticketing-system/commit/fc8ab7a933c9d1279a7ed97ebb906eb88eebf93c))

## [1.8.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.8.0...v1.8.1) (2023-02-20)


### Bug Fixes

* return 404 when ticket not found ([7ad585a](https://github.com/informatievlaanderen/ticketing-system/commit/7ad585a9e1e0b3fed29f6f290b54c96f427b46b2))

# [1.8.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.13...v1.8.0) (2023-01-27)


### Bug Fixes

* fix build ([7990654](https://github.com/informatievlaanderen/ticketing-system/commit/799065423de008162479a807c95bf7d075f342e1))
* remove ([8e0eccb](https://github.com/informatievlaanderen/ticketing-system/commit/8e0eccb080dda648958fc1ea343c1f861e634689))


### Features

* add monitoring proj ([16fb857](https://github.com/informatievlaanderen/ticketing-system/commit/16fb85752833ad16cc9c780cd8efdb0e10958169))

## [1.7.13](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.12...v1.7.13) (2022-11-03)


### Bug Fixes

* add nuget to dependabot.yml ([6088659](https://github.com/informatievlaanderen/ticketing-system/commit/60886599f3b7e1d5ceac2e2f93f78c318e270fd2))
* remove language & target framework from Storage.PgSqlMarten ([6a6d927](https://github.com/informatievlaanderen/ticketing-system/commit/6a6d9270fe553fcf58312f01a2f8404df4ab369e))
* remove paket.dependencies & comments ([daa0122](https://github.com/informatievlaanderen/ticketing-system/commit/daa0122dbf48eb9f3eb75df5f799e479938de98d))
* update ci & test branch protection ([0deb152](https://github.com/informatievlaanderen/ticketing-system/commit/0deb152d25915ed43881707c971b11f3925588c0))
* use central package management ([81ed656](https://github.com/informatievlaanderen/ticketing-system/commit/81ed656060196f82d9cc37a734bae3cd2b2e78ff))
* use VBR_SONAR_TOKEN ([5961ba7](https://github.com/informatievlaanderen/ticketing-system/commit/5961ba75a45cf6b3c3d30645ce908773a1393083))

## [1.7.12](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.11...v1.7.12) (2022-10-29)


### Bug Fixes

* enable pr's & coverage ([#59](https://github.com/informatievlaanderen/ticketing-system/issues/59)) ([0602bff](https://github.com/informatievlaanderen/ticketing-system/commit/0602bff30fb7b402e1338932f92aeb0513a024be))

## [1.7.11](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.10...v1.7.11) (2022-10-03)


### Bug Fixes

* update ticketresult contract ([d46c43a](https://github.com/informatievlaanderen/ticketing-system/commit/d46c43a7dd2fa0554b182fcd69f8a9934fd8c783))

## [1.7.10](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.9...v1.7.10) (2022-10-03)


### Bug Fixes

* camelcase enums ([628c231](https://github.com/informatievlaanderen/ticketing-system/commit/628c231b3eccd08f06dd11b473de0ab463b1605e))

## [1.7.9](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.8...v1.7.9) (2022-09-29)


### Reverts

* Revert "docs: update ticket field docs GAWR-3716" ([0cbb639](https://github.com/informatievlaanderen/ticketing-system/commit/0cbb63943aa628f56597f72cd31baed22eeb7c52))

## [1.7.8](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.7...v1.7.8) (2022-09-28)


### Bug Fixes

* don't return ticket Id when create ticket failed ([0731c98](https://github.com/informatievlaanderen/ticketing-system/commit/0731c98238d1aee32f7a58922a830a8a54a99653))

## [1.7.7](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.6...v1.7.7) (2022-09-28)


### Bug Fixes

* use camelcase for dictionaries ([301a6a7](https://github.com/informatievlaanderen/ticketing-system/commit/301a6a76fb458bfecb97350dddce975ae0f350a7))

## [1.7.6](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.5...v1.7.6) (2022-09-26)


### Bug Fixes

* style to trigger build ([93b9c10](https://github.com/informatievlaanderen/ticketing-system/commit/93b9c107658ae1b6f255d4e194227eb63997a79d))

## [1.7.5](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.4...v1.7.5) (2022-09-26)


### Bug Fixes

* correct marten / api settings + rename ticketresult ([4714b64](https://github.com/informatievlaanderen/ticketing-system/commit/4714b64ffe84afc923937067addc5c5403322318))
* return datetimes as western europe datetimes ([a40427a](https://github.com/informatievlaanderen/ticketing-system/commit/a40427a08dfe7608b0157a3fb894ed2b5553682e))

## [1.7.4](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.3...v1.7.4) (2022-09-22)


### Bug Fixes

* update readme to trigger build ([81a7ad9](https://github.com/informatievlaanderen/ticketing-system/commit/81a7ad9ba460c13166bb65f010ad4295fa5658d1))

## [1.7.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.2...v1.7.3) (2022-09-22)


### Bug Fixes

* create readme file to trigger build ([8425be1](https://github.com/informatievlaanderen/ticketing-system/commit/8425be150c2451ca50fc9059e84d19a87dd839ea))

## [1.7.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.1...v1.7.2) (2022-09-22)

## [1.7.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.7.0...v1.7.1) (2022-09-08)


### Bug Fixes

* correct usage of httpclient ([d31459b](https://github.com/informatievlaanderen/ticketing-system/commit/d31459bf9cac5a3009f6d5ff57e9563374c363f8))

# [1.7.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.6.1...v1.7.0) (2022-09-08)


### Bug Fixes

* change workflow ([0b150c4](https://github.com/informatievlaanderen/ticketing-system/commit/0b150c4b42e68c3a82ce3994a23f6244138d58d5))
* remove release check ([c41f339](https://github.com/informatievlaanderen/ticketing-system/commit/c41f339f7cecc69111c442d2ce3c52d8e9c29fd2))


### Features

* add httpproxyticketing registration ([dafa051](https://github.com/informatievlaanderen/ticketing-system/commit/dafa05195064f3263630eb24b63997ef506c4a57))
* add ticketingurl ([9974196](https://github.com/informatievlaanderen/ticketing-system/commit/9974196859178994257fdae7bc648ea00bde47f0))

## [1.6.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.6.0...v1.6.1) (2022-09-06)

# [1.6.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.5.0...v1.6.0) (2022-09-01)


### Features

* faulted ticket ([867a3ed](https://github.com/informatievlaanderen/ticketing-system/commit/867a3ed3c9a82ee3bd0a8cd8f28e91bca6ed5940))

# [1.5.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.4.0...v1.5.0) (2022-09-01)


### Features

* add created and last modified timestamps on a ticket ([#41](https://github.com/informatievlaanderen/ticketing-system/issues/41)) ([55fbed9](https://github.com/informatievlaanderen/ticketing-system/commit/55fbed965c16cdffd28b2afb7ade84b3ac0daf7a))

# [1.4.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.3.4...v1.4.0) (2022-07-29)


### Features

* add healthchecks ([f7d092a](https://github.com/informatievlaanderen/ticketing-system/commit/f7d092a4a4181029ff767a150d605ffe470cf394))

## [1.3.4](https://github.com/informatievlaanderen/ticketing-system/compare/v1.3.3...v1.3.4) (2022-07-20)


### Bug Fixes

* add security ([08eea4b](https://github.com/informatievlaanderen/ticketing-system/commit/08eea4bef0b78b8cc48cfd61b11158f21c90ee19))

## [1.3.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.3.2...v1.3.3) (2022-07-18)


### Bug Fixes

* refine ticketing service ([634dafc](https://github.com/informatievlaanderen/ticketing-system/commit/634dafcf0ef79e0aa2aa4dd118bdc13bafcf5fd0))

## [1.3.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.3.1...v1.3.2) (2022-07-15)


### Bug Fixes

* use postgres container for testing ([367e02c](https://github.com/informatievlaanderen/ticketing-system/commit/367e02cecfa58fc7531fd2bba0c4e17e943ffdba))

## [1.3.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.3.0...v1.3.1) (2022-07-15)


### Bug Fixes

* fake commit ([5a2edd6](https://github.com/informatievlaanderen/ticketing-system/commit/5a2edd633285d31c7ae9c84c2ece8945d7456c22))

# [1.3.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.2.0...v1.3.0) (2022-07-14)


### Features

* add ITicketingUrl ([937dbf6](https://github.com/informatievlaanderen/ticketing-system/commit/937dbf671f6e628fd1bb8c0aa2f2449665d97d96))

# [1.2.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.1.1...v1.2.0) (2022-07-13)


### Features

* add http proxy ([2c0542b](https://github.com/informatievlaanderen/ticketing-system/commit/2c0542b3560ddd767125ed86acb73360b7c5b9f2))

## [1.1.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.1.0...v1.1.1) (2022-07-13)


### Bug Fixes

* push to nuget ([297d65f](https://github.com/informatievlaanderen/ticketing-system/commit/297d65fa35fbe1795ac67921edd5f8ea029013ae))

# [1.1.0](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.6...v1.1.0) (2022-07-13)


### Features

* add Postgres backend ([81d199c](https://github.com/informatievlaanderen/ticketing-system/commit/81d199c3bf4e973cad35131bf104cc89a4e698bd))

## [1.0.6](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.5...v1.0.6) (2022-07-08)


### Bug Fixes

* replace ticket result with object ([02dd603](https://github.com/informatievlaanderen/ticketing-system/commit/02dd603af1b1b68d49c50642f7d4eee381da82b6))

## [1.0.5](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.4...v1.0.5) (2022-07-05)


### Bug Fixes

* delete ticketing tests ([d8d9a81](https://github.com/informatievlaanderen/ticketing-system/commit/d8d9a81254823eb8f8370dcbce576270152fb298))

## [1.0.4](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.3...v1.0.4) (2022-07-05)


### Bug Fixes

* document tests ([5aba4f0](https://github.com/informatievlaanderen/ticketing-system/commit/5aba4f0798883fbaf767c18ca1e65b7f4643d13f))

## [1.0.3](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.2...v1.0.3) (2022-07-05)


### Bug Fixes

* document test ([5052da4](https://github.com/informatievlaanderen/ticketing-system/commit/5052da4d479c3f651c748b6d8fe9c8c2d1468376))

## [1.0.2](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.1...v1.0.2) (2022-07-05)


### Bug Fixes

* document test ([ed0bcda](https://github.com/informatievlaanderen/ticketing-system/commit/ed0bcda24029443dce9b504cb0c1614905e32b02))

## [1.0.1](https://github.com/informatievlaanderen/ticketing-system/compare/v1.0.0...v1.0.1) (2022-07-05)


### Bug Fixes

* sonar build ([434b2c3](https://github.com/informatievlaanderen/ticketing-system/commit/434b2c325e24cb75fd8655da399ceef11dabca02))

# 1.0.0 (2022-07-05)


### Bug Fixes

* add in-memory storage ([0fb7f2f](https://github.com/informatievlaanderen/ticketing-system/commit/0fb7f2f42346ccd3ef352bf3bee3549d2f67f74b))
* build.fsx & paket.template ([3c70582](https://github.com/informatievlaanderen/ticketing-system/commit/3c7058225e85e9692c866a9b774f220d4299c4ce))
* change buildingregistry ([c5e2665](https://github.com/informatievlaanderen/ticketing-system/commit/c5e266524934f52f7887473693aa6281178bc763))
* correct build restore step ([551c041](https://github.com/informatievlaanderen/ticketing-system/commit/551c0419b4a3905bfa369c8ce4171319da20d890))
* fill paket.template ([cff0615](https://github.com/informatievlaanderen/ticketing-system/commit/cff06159a208a1f7b0a9b1955e0ee98e20f2ea53))
* finalize call interface ([6112bad](https://github.com/informatievlaanderen/ticketing-system/commit/6112bad5cd682b9b357ab40c37b00a82ab881cd2))
* remove building-registry references ([708cfb0](https://github.com/informatievlaanderen/ticketing-system/commit/708cfb0cca030322b1d43c443609d020c228d46f))
* update build.sh index ([919d1b2](https://github.com/informatievlaanderen/ticketing-system/commit/919d1b29ea1f04d341853703df493c2572419414))
* warning publish-duplicate-output-files ([588a7d5](https://github.com/informatievlaanderen/ticketing-system/commit/588a7d5c30fb6f744c7cc47caceab0798cdb7961))


### Features

* initial commit ([b0bcf8d](https://github.com/informatievlaanderen/ticketing-system/commit/b0bcf8d93b18aba5c2ee3c10a854b3139df383c6))
