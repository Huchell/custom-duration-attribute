@echo off

set "command=nvim"
set "workdir=."

set "ReturnValue="
set "ProcessId="
for /f " skip=5 eol=} tokens=* delims=" %%a in ('wmic process call create "%command%"^, "%workdir%"') do (
	for /f "tokens=1,3 delims=; " %%c in ("%%a") do (
		set "%%c=%%d"
	)
)

if not %ReturnValue%==0 (
	echo error - %ReturnValue%
) else if defined ProcessId echo PID -^> %ProcessId%
