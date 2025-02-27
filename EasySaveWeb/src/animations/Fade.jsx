import React from "react";
import { useState, useEffect } from "react";

const VISIBLE = 1;
const HIDDEN = 2;
const ENTERING = 3;
const LEAVING = 4;

export function Fade({visible, children, duration = 300}){
    const className = visible ? "fade" : "fade out";
    return <div className={className}>{children}</div>
}