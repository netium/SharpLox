#pragma once

#ifndef CLOX_COMPILER_H
#define CLOX_COMPILER_H

#include "object.h"
#include "vm.h"
#include "chunk.h"

bool compile(const char* source, Chunk *chunk);

#endif