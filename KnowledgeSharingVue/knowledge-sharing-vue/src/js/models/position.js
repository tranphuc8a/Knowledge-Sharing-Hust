/* eslint-disable */

import { Entity } from "./entity";

class Position extends Entity{

    constructor(positionApi){
        super(positionApi);
    }

    initProperties(){
        super.initProperties();
        this.keys = [...this.keys, "PositionId", "PositionCode", "PositionName"];
    }

}

export { Position };