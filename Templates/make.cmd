@echo off
for /d %%i in (*) do finkgo c %%i.nkar %%i.manifest %%i/*