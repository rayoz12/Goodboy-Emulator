const fs = require("fs");

const opcodes = JSON.parse(fs.readFileSync("Opcodes.json", "utf-8"))
let counter = 0;

for (const key in opcodes.unprefixed) {
    const element = opcodes.unprefixed[key];
    const dissassembly = element.mnemonic + " " + element.operands.map(it => it.name).join(", ");
    const byteLiteral = counter.toString(16).toUpperCase().padStart(2, "0");
    console.log(`            new InstructionReference("${dissassembly}", ${element.bytes - 1}),		// 0x${byteLiteral}`);
    counter++;
}

counter = 0;
for (const key in opcodes.cbprefixed) {
    const element = opcodes.cbprefixed[key];
    const dissassembly = element.mnemonic + " " + element.operands.map(it => it.name).join(", ");
    const byteLiteral = counter.toString(16).toUpperCase().padStart(2, "0");
    console.log(`            new InstructionReference("${dissassembly}", ${element.bytes - 1}),		// 0x${byteLiteral}`);
    counter++;
}